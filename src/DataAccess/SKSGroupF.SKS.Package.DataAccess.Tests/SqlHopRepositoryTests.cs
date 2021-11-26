using FizzWare.NBuilder;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using SKSGroupF.SKS.Package.DataAccess.Interfaces.Exceptions;
using SKSGroupF.SKS.Package.DataAccess.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.DataAccess.Tests
{
    class SqlHopRepositoryTests
    {
        private IHopRepository repo;
        List<DALHop> hops = new List<DALHop>();
        DALHop validHop;

        [SetUp]
        public void Setup()
        {
            hops = Builder<DALHop>.CreateListOfSize(10).Build().ToList();
            var geoCoors = Builder<DALGeoCoordinate>.CreateListOfSize(1).Build().ToList();
            var wnhs = Builder<DALWarehouseNextHops>.CreateListOfSize(1).Build().ToList();
            validHop = Builder<DALHop>.CreateNew().With(p => p.Code = "ABCD\\dddd").Build();

            var DBMock = new Mock<ISqlDbContext>();
            DBMock.Setup(p => p.DbHop).Returns(SqlDbContextMock.GetQueryableMockDbSet(hops));
            DBMock.Setup(p => p.DbGeoCoordinate).Returns(SqlDbContextMock.GetQueryableMockDbSet(geoCoors));
            DBMock.Setup(p => p.DbWarehouseNextHops).Returns(SqlDbContextMock.GetQueryableMockDbSet(wnhs));
            DBMock.Setup(p => p.SaveChangesToDb()).Returns(1);

            repo = new SqlHopRepository(DBMock.Object, new NullLogger<SqlHopRepository>());
        }

        [Test]
        public void Create_InsertsHopIntoDB_IncreasesDbCountByOne()
        {
            var counter = hops.Count + 1;

            try
            {
                repo.Create(validHop);
            }
            catch { }

            Assert.AreEqual(counter, hops.Count);
        }

        [Test]
        public void Create_InsertsHopIntoDB_InsertsHopCorrectly()
        {
            validHop.Id = 5;
            validHop.Code = "ACDC";
            var expected = validHop.Code;

            try
            {
                repo.Create(validHop);
            }
            catch { }

            Assert.AreEqual(expected, hops.Last().Code);
        }

        [Test]
        public void Update_UpdatesHopInDb_AppliesChangesCorrectly()
        {
            validHop.Id = 5;
            validHop.Code = "ACDC";
            var expected = "gggg";

            try
            {
                repo.Create(validHop);
            }
            catch { }

            validHop.Code = "gggg";
            repo.Update(validHop);

            Assert.AreEqual(expected, hops.Last().Code);
        }

        [Test]
        public void Update_CannotFindHopInDatabase_ThrowsDataException()
        {
            Assert.Throws<DALDataException>(() => repo.Update(new DALHop()));
        }

        [Test]
        public void Delete_DeletesHopWithId_DecreasesDbCountByOne()
        {
            var counter = hops.Count - 1;

            repo.Delete(2);

            Assert.AreEqual(counter, hops.Count);
        }

        [Test]
        public void Delete_DeletesHopWithId_CorrectParcelRemovedFromDb()
        {
            var expected1 = 1;
            var expected2 = 3;

            repo.Delete(2);

            Assert.AreEqual(expected1, hops[0].Id);
            Assert.AreEqual(expected2, hops[1].Id);
        }

        [Test]
        public void Delete_CannotDeleteSpecifiedHop_ThrowsDataNotFoundException()
        {
            Assert.Throws<DALDataNotFoundException>(() => repo.Delete(20));
        }

        [Test]
        public void GetAll_SelectsAllHopsFromDb_SelectsHopsCorrectly()
        {
            var hopList = repo.GetAll();

            Assert.AreEqual(hops.Count, hopList.ToList().Count);
        }

        [Test]
        public void GetAll_CannotFindAnyHops_ThrowsDataNotFoundException()
        {
            var DBMock = new Mock<ISqlDbContext>();
            DBMock.Setup(p => p.DbHop).Throws(new Exception());

            repo = new SqlHopRepository(DBMock.Object, new NullLogger<SqlHopRepository>());

            Assert.Throws<DALDataNotFoundException>(() => repo.GetAll());
        }

        [Test]
        public void GetByCode_SelectsHopWithValidCode_SelectsCorrectHop()
        {
            var hop = repo.GetByCode(hops[1].Code);

            Assert.AreEqual(hops[1].Code, hop.Code);
        }
    }
}
