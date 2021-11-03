﻿using FizzWare.NBuilder;
using Moq;
using NUnit.Framework;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
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
            validHop = Builder<DALHop>.CreateNew().With(p => p.Code = "ABCD\\dddd").Build();

            var DBMock = new Mock<ISqlDbContext>();
            DBMock.Setup(p => p.DbHop).Returns(SqlDbContextMock.GetQueryableMockDbSet(hops));
            DBMock.Setup(p => p.SaveChangesToDb()).Returns(1);

            repo = new SqlHopRepository(DBMock.Object);
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
    }
}
