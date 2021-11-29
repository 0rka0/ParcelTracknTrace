using AutoMapper;
using FizzWare.NBuilder;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using SKSGroupF.SKS.Package.BusinessLogic.Logic;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using SKSGroupF.SKS.Package.Webhooks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKSGroupF.SKS.Package.BusinessLogic.Tests
{
    class WarehouseLogicTests
    {
        private IWarehouseLogic logic;
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new BlDalProfiles());
            });
            mapper = config.CreateMapper();

            var hopList = Builder<DALHop>.CreateListOfSize(3).Build().ToList();
            var hop = new DALWarehouse();
            hop.Code = "ABCD04";

            Mock<IParcelRepository> mockParcelRepo = new();

            Mock<IHopRepository> mockHopRepo = new();
            mockHopRepo.Setup(m => m.Create(It.IsAny<DataAccess.Entities.Models.DALHop>())).Returns(1);
            mockHopRepo.Setup(m => m.GetAll()).Returns(hopList);
            mockHopRepo.Setup(m => m.GetByCode(It.IsAny<string>())).Returns(hop);

            Mock<IWebhookRepository> mockWebhookRepo = new();
            Mock<IWebhookManager> mockManager = new();

            logic = new WarehouseLogic(mapper, mockParcelRepo.Object, mockHopRepo.Object, mockWebhookRepo.Object, mockManager.Object, new NullLogger<WarehouseLogic>());
        }

        [Test]
        public void ExportWarehouses_ReturnsListOfWarehouses()
        {
            List<BLHop> warehouseList = (List<BLHop>)logic.ExportWarehouses();

            Assert.IsNotNull(warehouseList);
            Assert.AreEqual(3, warehouseList.Count);
        }

        [Test]
        public void GetWarehouse_ReceivesInvalidCode_ThrowsException()
        {
            Assert.Throws<BLLogicException>(() => logic.GetWarehouse("ABCD\\ddddd"));
        }

        [Test]
        public void GetWarehouse_ReceivesValidCode_ReturnsWarehouseObjectWithCode()
        {
            string code = "ABCD04";
            var warehouse = logic.GetWarehouse(code);

            Assert.IsNotNull(warehouse);
            Assert.AreEqual(code, warehouse.Code);
        }

        [Test]
        public void ImportWarehouses_ReceivesInvalidWarehouse_ThrowsException()
        {
            Assert.Throws<BLLogicException>(() => logic.ImportWarehouses(new BLWarehouse()));
        }

        [Test]
        public void ImportWarehouses_ReceivesValidWarehouse_RunsWithoutException()
        {
            var validWarehouse = Builder<BLWarehouse>.CreateNew()
                .With(p => p.Description = "Lager1")
                .With(p => p.NextHops = Builder<BLWarehouseNextHops>.CreateListOfSize(3).Build().ToList())
                .With(p => p.LocationCoordinates = Builder<BLGeoCoordinate>.CreateNew().Build())
                .Build();

            logic.ImportWarehouses(validWarehouse);

            Assert.IsTrue(true);
        }
    }
}
