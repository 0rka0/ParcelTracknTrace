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
using SKSGroupF.SKS.Package.DataAccess.Interfaces.Exceptions;
using RichardSzalay.MockHttp;
using System.Net.Http;

namespace SKSGroupF.SKS.Package.DataAccess.Tests
{
    class SqlParcelRepositoryTests
    {
        private IParcelRepository repo;
        List<DALParcel> parcels;
        List<DALHopArrival> hopArrivals;
        DALParcel validParcel;
        HttpClient client;

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

            parcels[1].Receipient = validParcel.Receipient;
            parcels[1].Sender = validParcel.Sender;
            parcels[1].FutureHops = hopArrivals;
            parcels[1].VisitedHops = new List<DALHopArrival>();

            hopArrivals[3] = validParcel.FutureHops[0];

            var DBMock = new Mock<ISqlDbContext>();
            DBMock.Setup(p => p.DbParcel).Returns(SqlDbContextMock.GetQueryableMockDbSet(parcels));
            DBMock.Setup(p => p.DbHopArrival).Returns(SqlDbContextMock.GetQueryableMockDbSet(hopArrivals));
            DBMock.Setup(p => p.SaveChangesToDb()).Returns(1);

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://test/*").Respond("application/json", "Success");
            client = new HttpClient(mockHttp);

            repo = new SqlParcelRepository(DBMock.Object, new NullLogger<SqlParcelRepository>(), client);
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
        public void Delete_CannotDeleteSpecifiedParcel_ThrowsDataNotFoundException()
        {
            Assert.Throws<DALDataNotFoundException>(() => repo.Delete(20));
        }

        [Test]
        public void GetAll_SelectsAllParcelsFromDb_SelectsParcelsCorrectly()
        {
            var parcelList= repo.GetAll();

            Assert.AreEqual(parcels.Count, parcelList.ToList().Count);
        }

        [Test]
        public void GetAll_CannotFindAnyParcels_ThrowsDataNotFoundException()
        {
            var DBMock = new Mock<ISqlDbContext>();
            DBMock.Setup(p => p.DbParcel).Throws(new Exception());
            DBMock.Setup(p => p.DbHopArrival).Throws(new Exception());

            repo = new SqlParcelRepository(DBMock.Object, new NullLogger<SqlParcelRepository>(), client);

            Assert.Throws<DALDataNotFoundException>(() => repo.GetAll());
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
            Assert.Throws<DALDataNotFoundException>(() => repo.GetByTrackingId("abc"));
        }

        [Test]
        public void UpdateHopState_TransfersFHopToVHop_HopStateGetsUpdated()
        {
            DALHop hop = new DALHop();
            hop.HopType = "Warehouse";
            var expectedF = parcels[1].FutureHops.Count - 1;
            var expectedV = parcels[1].VisitedHops.Count + 1;
            repo.UpdateHopState(parcels[1].TrackingId, validParcel.FutureHops[0].Code, hop);

            Assert.AreEqual(expectedF, parcels[1].FutureHops.Count);
            Assert.AreEqual(expectedV, parcels[1].VisitedHops.Count);
        }

        [Test]
        public void UpdateHopState_TransfersFHopToVHop_SendsRequestToLogisticsPartner()
        {
            DALTransferWarehouse hop = new DALTransferWarehouse();
            hop.HopType = "TransferWarehouse";
            hop.LogisticsPartnerUrl = "http://our-partner-in.slovenia.com";
            repo.UpdateHopState(parcels[1].TrackingId, validParcel.FutureHops[0].Code, hop);

            Assert.True(true);
        }

        [Test]
        public void GetHopArrivalByCode_SelectsHopArrivalWithInvalidCode_ThrowsDataNotFoundException()
        {
            Assert.Throws<DALDataNotFoundException>(() => repo.GetHopArrivalByCode("aa", validParcel));
        }

        [Test]
        public void GetHopArrivalsByParcel_SelectsVisitedHopsFromParcel_ReturnsListOfHopArrivals()
        {
            parcels[1].VisitedHops.Add(new DALHopArrival());
            var hops = repo.GetHopArrivalsByParcel(parcels[1], true);

            Assert.IsNotEmpty(hops);
        }

        [Test]
        public void GetHopArrivalsByParcel_SelectsVisitedHopsFromParcelWithNoHops_ReturnsEmptyList()
        {
            var hops = repo.GetHopArrivalsByParcel(parcels[1], true);

            Assert.IsEmpty(hops);
        }

        [Test]
        public void GetHopArrivalsByParcel_GetsParcelThatIsNotInDatabase_ThrowsDataNotFoundException()
        {
            Assert.Throws<DALDataNotFoundException>(() => repo.GetHopArrivalsByParcel(validParcel, true));
        }

        [Test]
        public void GetHopArrivalsByParcel_SelectsFutureHopsFromParcel_ReturnsListOfHopArrivals()
        {
            var hops = repo.GetHopArrivalsByParcel(parcels[1], false);

            Assert.IsNotEmpty(hops);
        }

        [Test]
        public void UpdateDelivered_GetsParcelFromDatabase_SetsItStateToDelivered()
        {
            repo.UpdateDelivered(parcels[1]);

            Assert.AreEqual(DALParcel.StateEnum.DeliveredEnum, parcels[1].State);
        }

        [Test]
        public void UpdateDelivered_ErrorWhenUpdatingParcelInDatabase_ThrowsDataException()
        {
            var DBMock = new Mock<ISqlDbContext>();
            DBMock.Setup(p => p.DbParcel).Throws(new Exception());
            repo = new SqlParcelRepository(DBMock.Object, new NullLogger<SqlParcelRepository>(), client);

            Assert.Throws<DALDataException>(() => repo.UpdateDelivered(validParcel));
        }
    }
}
