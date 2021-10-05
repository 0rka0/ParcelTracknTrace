using NUnit.Framework;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Logic;
using System;

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
    }
}
