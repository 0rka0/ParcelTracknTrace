using FizzWare.NBuilder;
using NUnit.Framework;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKSGroupF.SKS.Package.BusinessLogic.Tests
{
    class WarehouseLogicTests
    {
        private IWarehouseLogic logic;

        [SetUp]
        public void Setup()
        {
            logic = new WarehouseLogic();
        }

        [Test]
        public void ExportWarehouses_ReturnsListOfWarehouses()
        {
            List<BLWarehouse> warehouseList = (List<BLWarehouse>)logic.ExportWarehouses();

            Assert.IsNotNull(warehouseList);
            Assert.AreEqual(3, warehouseList.Count);
        }

        [Test]
        public void GetWarehouse_ReceivesInvalidCode_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => logic.GetWarehouse("ABCD\\ddddd"));
        }

        [Test]
        public void GetWarehouse_ReceivesValidCode_ReturnsWarehouseObjectWithCode()
        {
            string code = "ABCD\\dddd";
            var warehouse = logic.GetWarehouse(code);

            Assert.IsNotNull(warehouse);
            Assert.AreEqual(code, warehouse.Code);
        }

        [Test]
        public void ImportWarehouses_ReceivesInvalidWarehouse_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => logic.ImportWarehouses(new BLWarehouse()));
        }

        [Test]
        public void ImportWarehouses_ReceivesValidWarehouse_RunsWithoutException()
        {
            var validWarehouse = Builder<BLWarehouse>.CreateNew()
                .With(p => p.Description = "Lager1")
                .With(p => p.NextHops = Builder<BLWarehouseNextHops>.CreateListOfSize(3).Build().ToList())
                .Build();

            logic.ImportWarehouses(validWarehouse);

            Assert.IsTrue(true);
        }
    }
}
