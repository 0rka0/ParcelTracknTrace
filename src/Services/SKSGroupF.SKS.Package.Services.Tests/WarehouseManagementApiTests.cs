using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using SKSGroupF.SKS.Package.Services.Controllers;
using SKSGroupF.SKS.Package.Services.DTOs.Models;
using System;
using System.Collections.Generic;
using AutoMapper;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using Moq;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using FizzWare.NBuilder;
using Microsoft.Extensions.Logging.Abstractions;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces.Exceptions;

namespace SKSGroupF.SKS.Package.Services.Test
{
    public class WarehouseManagementApiTests
    {
        private IMapper mapper;
        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new SvcBlProfiles());
            });
            mapper = config.CreateMapper();
        }

        [Test]
        public void ExportWarehouses_ExceptionThrown_ReturnsErrorStatusCode()
        {
            Mock<IWarehouseLogic> mockLogic = new();
            mockLogic.Setup(m => m.ExportWarehouses()).Throws(new Exception());

            WarehouseManagementApiController controller = new WarehouseManagementApiController(mapper, mockLogic.Object, new NullLogger<WarehouseManagementApiController>());

            ObjectResult result = (ObjectResult)controller.ExportWarehouses();

            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public void ExportWarehouses_Runs_ReturnsOkayStatusCode()
        {
            Mock<IWarehouseLogic> mockLogic = new();
            mockLogic.Setup(m => m.ExportWarehouses()).Returns(new List<BLHop> { new BLWarehouse(), new BLWarehouse(), new BLWarehouse() });

            WarehouseManagementApiController controller = new WarehouseManagementApiController(mapper, mockLogic.Object, new NullLogger<WarehouseManagementApiController>());

            ObjectResult result = (ObjectResult)controller.ExportWarehouses();

            Assert.NotNull(result.Value);
            Assert.AreEqual(3, ((List<Hop>)result.Value).Count);
        }

        [Test]
        public void GetWarehouse_BLGetsInvalidData_ReturnsErrorStatusCode()
        {
            Mock<IWarehouseLogic> mockLogic = new();
            mockLogic.Setup(m => m.GetWarehouse(It.IsNotIn("ABCD\\dddd"))).Throws(new BLLogicException(nameof(WarehouseManagementApiTests)));

            WarehouseManagementApiController controller = new WarehouseManagementApiController(mapper, mockLogic.Object, new NullLogger<WarehouseManagementApiController>());

            var result = (ObjectResult)controller.GetWarehouse("ABCD\\ddddd");

            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void GetWarehouse_BLGetsValidData_ReturnsWarehouseObjectWithCode()
        {
            var mockBlWarehouse = new BLWarehouse();
            mockBlWarehouse.Code = "ABCD\\dddd";
            Mock<IWarehouseLogic> mockLogic = new();
            mockLogic.Setup(m => m.GetWarehouse(It.IsIn("ABCD\\dddd"))).Returns(mockBlWarehouse);

            WarehouseManagementApiController controller = new WarehouseManagementApiController(mapper, mockLogic.Object, new NullLogger<WarehouseManagementApiController>());

            var result = (ObjectResult)controller.GetWarehouse("ABCD\\dddd");

            Assert.NotNull(result.Value);
            Assert.AreEqual("ABCD\\dddd", ((Warehouse)result.Value).Code);
        }

        [Test]
        public void ImportWarehouses_BLGetsInvalidData_ReturnsErrorStatusCode()
        {
            Mock<IWarehouseLogic> mockLogic = new();
            mockLogic.Setup(m => m.ImportWarehouses(It.IsAny<BLWarehouse>())).Throws(new ArgumentOutOfRangeException());

            WarehouseManagementApiController controller = new WarehouseManagementApiController(mapper, mockLogic.Object, new NullLogger<WarehouseManagementApiController>());

            var warehouse = Builder<Warehouse>.CreateNew()
                .With(p => p.NextHops = null)
                .Build();

            var result = (ObjectResult)controller.ImportWarehouses(warehouse);

            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public void ImportWarehouses_ValidData()
        {
            Mock<IWarehouseLogic> mockLogic = new();
            mockLogic.Setup(m => m.ImportWarehouses(It.IsAny<BLWarehouse>()));

            WarehouseManagementApiController controller = new WarehouseManagementApiController(mapper, mockLogic.Object, new NullLogger<WarehouseManagementApiController>());

            var warehouse = Builder<Warehouse>.CreateNew()
                .With(p => p.NextHops = null)
                .Build();

            var result = (StatusCodeResult)controller.ImportWarehouses(warehouse);

            Assert.AreEqual(200, result.StatusCode);
        }
    }
}