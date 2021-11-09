using NUnit.Framework;
using Moq;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using SKSGroupF.SKS.Package.DataAccess.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using FizzWare.NBuilder;
using Microsoft.Extensions.Logging.Abstractions;

namespace SKSGroupF.SKS.Package.DataAccess.Tests
{
    class SqlParcelRepositoryTests
    {
        private IParcelRepository repo;
        List<DALParcel> parcels;
        List<DALHopArrival> hopArrivals;
        DALParcel validParcel;

        [SetUp]
        public void Setup()
        {
            validParcel = Builder<DALParcel>.CreateNew()
                .With(p => p.Receipient = Builder<DALReceipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DALReceipient>.CreateNew().Build())
                .With(p => p.Weight = 5.0f)
                .With(p => p.FutureHops = Builder<DALHopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.VisitedHops = Builder<DALHopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.State = DALParcel.StateEnum.InTransportEnum)
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .Build();

            validParcel.FutureHops[0].Code = "abcd";

            parcels = Builder<DALParcel>.CreateListOfSize(3).Build().ToList();
            foreach (DALParcel p in parcels)
            {
                p.State = DALParcel.StateEnum.InTransportEnum;
                p.Weight = 3.0f;
            }
            hopArrivals = Builder<DALHopArrival>.CreateListOfSize(10).Build().ToList();

            hopArrivals[3] = validParcel.FutureHops[0];

            var DBMock = new Mock<ISqlDbContext>();
            DBMock.Setup(p => p.DbParcel).Returns(SqlDbContextMock.GetQueryableMockDbSet(parcels));
            DBMock.Setup(p => p.DbHopArrival).Returns(SqlDbContextMock.GetQueryableMockDbSet(hopArrivals));
            DBMock.Setup(p => p.SaveChangesToDb()).Returns(1);

            repo = new SqlParcelRepository(DBMock.Object, new NullLogger<SqlParcelRepository>());
        }

        [Test]
        public void Create_InsertsParcelIntoDB_IncreasesDbCountByOne()
        {
            var counter = parcels.Count + 1;

            try
            {
                repo.Create(validParcel);
            }
            catch { }

            Assert.AreEqual(counter, parcels.Count);
        }

        [Test]
        public void Create_InsertsParcelIntoDB_InsertsParcelCorrectly()
        {
            validParcel.TrackingId = "aaaaa";
            validParcel.Id = 4;
            var expected = validParcel.TrackingId;

            try
            {
                repo.Create(validParcel);
            }
            catch { }

            Assert.AreEqual(expected, parcels.Last().TrackingId);
        }

        [Test]
        public void Update_UpdatesParcelInDb_AppliesChangesCorrectly()
        {
            validParcel.TrackingId = "aaaaa";
            validParcel.Id = 4;
            var expected = "bbbbb";

            try
            {
                repo.Create(validParcel);
            }
            catch { }

            validParcel.TrackingId = "bbbbb";
            repo.Update(validParcel);

            Assert.AreEqual(expected, parcels.Last().TrackingId);
        }

        [Test]
        public void Delete_DeletesParcelWithId_DecreasesDbCountByOne()
        {
            var counter = parcels.Count - 1;

            repo.Delete(2);

            Assert.AreEqual(counter, parcels.Count);
        }

        [Test]
        public void Delete_DeletesParcelWithId_CorrectParcelRemovedFromDb()
        {
            var expected1 = 1;
            var expected2 = 3;

            repo.Delete(2);

            Assert.AreEqual(expected1, parcels[0].Id);
            Assert.AreEqual(expected2, parcels[1].Id);
        }

        [Test]
        public void GetAll_SelectsAllParcelsFromDb_SelectsParcelsCorrectly()
        {
            var parcelList= repo.GetAll();

            Assert.AreEqual(parcels.Count, parcelList.ToList().Count);
        }

        [Test]
        public void GetByTrackingId_SelectsParcelWithValidTid_SelectsCorrectParcel()
        {
            var parcel = repo.GetByTrackingId(parcels[1].TrackingId);

            Assert.AreEqual(parcels[1].TrackingId, parcel.TrackingId);
        }

        [Test]
        public void GetByTrackingId_SelectsParcelWithNonexistingTid_ReturnsNull()
        {
            var parcel = repo.GetByTrackingId("abc");

            Assert.IsNull(parcel);
        }

        [Test]
        public void GetByReceipient_SelectsParcelWithValidRec_SelectsCorrectParcels()
        {
            var expected = parcels.Where(p => p.Receipient == parcels[1].Receipient);
            var parcelList = repo.GetByReceipient(parcels[1].Receipient);

            Assert.AreEqual(expected, parcelList);
        }

        [Test]
        public void GetByReceipient_SelectsParcelWithNonexistingRec_ReturnsNull()
        {
            var parcelList = repo.GetByReceipient(new DALReceipient());

            Assert.IsEmpty(parcelList);
        }

        [Test]
        public void GetBySender_SelectsParcelWithValidRec_SelectsCorrectParcels()
        {
            var expected = parcels.Where(p => p.Sender == parcels[1].Sender);
            var parcelList = repo.GetBySender(parcels[1].Sender);

            Assert.AreEqual(expected, parcelList);
        }

        [Test]
        public void GetBySender_SelectsParcelWithNonexistingRec_ReturnsNull()
        {
            var parcelList = repo.GetBySender(new DALReceipient());

            Assert.IsEmpty(parcelList);
        }

        [Test]
        public void GetByState_SelectsParcelWithValidState_SelectsCorrectParcels()
        {
            var expected = parcels.Where(p => p.State == parcels[1].State);
            var parcelList = repo.GetByState(parcels[1].State.Value);

            Assert.AreEqual(expected, parcelList);
        }

        [Test]
        public void GetByState_SelectsParcelWithNonexistingState_ReturnsNull()
        {
            var parcelList = repo.GetByState(DALParcel.StateEnum.InTruckDeliveryEnum);

            Assert.IsEmpty(parcelList);
        }

        [Test]
        public void GetByWeight_SelectsParcelWithValidWeight_SelectsCorrectParcels()
        {
            var expected = parcels.Where(p => p.Weight == parcels[1].Weight);
            var parcelList = repo.GetByWeight(3.0f, 3.0f);

            Assert.AreEqual(expected, parcelList);
        }

        [Test]
        public void GetByWeight_SelectsParcelWithNonexistingWeight_ReturnsNull()
        {
            var parcelList = repo.GetByWeight(0.0f, 1.0f);

            Assert.IsEmpty(parcelList);
        }

        [Test]
        public void UpdateHopState_GetsParcelAndCode_TransfersFutureHopToVisitedHop()
        {
            var expectedF = validParcel.FutureHops.Count - 1;
            var expectedV = validParcel.VisitedHops.Count + 1;
            repo.UpdateHopState(validParcel, "abcd");

            Assert.AreEqual(expectedF, validParcel.FutureHops.Count);
            Assert.AreEqual(expectedV, validParcel.VisitedHops.Count);
        }
    }
}
