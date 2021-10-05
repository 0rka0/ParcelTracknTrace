using FizzWare.NBuilder;
using NUnit.Framework;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Logic;
using System;

namespace SKSGroupF.SKS.Package.BusinessLogic.Tests
{
    public class Tests
    {
        private IParcelLogic logic;

        [SetUp]
        public void Setup()
        {
            logic = new ParcelLogic();
        }

        [Test]
        public void SubmitParcel_ReceivesInvalidParcel_ThrowsException()
        {
            var parcel = Builder<BLParcel>.CreateNew()
                .With(p => p.Receipient = Builder<BLReceipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<BLReceipient>.CreateNew().Build())
                .With(p => p.Weight = 0)
                .Build();

            string trackingId = logic.SubmitParcel(parcel);

            
        }
    }
}