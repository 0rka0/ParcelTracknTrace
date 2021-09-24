using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using SKSGroupF.SKS.Package.Services.Controllers;
using SKSGroupF.SKS.Package.Services.DTOs.Models;
using System;
using System.Collections.Generic;

namespace SKSGroupF.SKS.Package.Services.Test
{
    public class WarehouseManagementApiTests
    {
        private WarehouseManagementApiController controller;
        [SetUp]
        public void Setup()
        {
            this.controller = new WarehouseManagementApiController();
        }

        [Test]
        public void ExportWarehouses_NoException()
        {
            ObjectResult result = (ObjectResult)controller.ExportWarehouses();
            Assert.NotNull(result);
        }

       [Test]
        public void GetWarehouse_InvalidData_ThrowsOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => controller.GetWarehouse("ABCD\\aaaa"));
        }

        [Test]
        public void GetWarehouse_ValidData()
        {
            ObjectResult result = (ObjectResult)controller.ExportWarehouses();
            Assert.NotNull(result);
        }

        [Test]
        public void ImportWarehouses_InvalidDataCode_ThrowsOutOfRangeException()
        {
            Warehouse warehouse = new Warehouse();
            warehouse.NextHops = null;
            Assert.Throws<ArgumentOutOfRangeException>(() => controller.ImportWarehouses(warehouse));
        }

        [Test]
        public void ImportWarehouses_ValidData()
        {
            Warehouse warehouse = new Warehouse();
            warehouse.NextHops = new List<WarehouseNextHops>();
            Assert.Throws<NotImplementedException>(() => controller.ImportWarehouses(warehouse));
        }
    }
}