using FizzWare.NBuilder;
using NUnit.Framework;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Logic;
using System;
using System.Linq;

namespace SKSGroupF.SKS.Package.BusinessLogic.Tests
{
    public class ParcelLogicTests
    {
        private IParcelLogic logic;
        private BLParcel validParcel;
        private BLParcel invalidParcel;

        [SetUp]
        public void Setup()
        {
            logic = new ParcelLogic();

            validParcel = Builder<BLParcel>.CreateNew()
                .With(p => p.Receipient = Builder<BLReceipient>.CreateNew()
                    .With(p => p.Country = "Austria")
                    .With(p => p.City = "Stadt")
                    .With(p => p.Name = "Name")
                    .With(p => p.PostalCode = "A-0000")
                    .With(p => p.Street = "Straße 1")
                    .Build())
                .With(p => p.Sender = Builder<BLReceipient>.CreateNew()
                    .With(p => p.Country = "Austria")
                    .With(p => p.City = "Stadt")
                    .With(p => p.Name = "Name")
                    .With(p => p.PostalCode = "A-0000")
                    .With(p => p.Street = "Straße 1")
                    .Build())
                .With(p => p.Weight = 5.0f)
                .With(p => p.FutureHops = Builder<BLHopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.VisitedHops = Builder<BLHopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.State = BLParcel.StateEnum.InTransportEnum)
                .Build();

            invalidParcel = Builder<BLParcel>.CreateNew()
                .With(p => p.Receipient = Builder<BLReceipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<BLReceipient>.CreateNew().Build())
                .With(p => p.Weight = 0)
                .Build();
        }

        [Test]
        public void SubmitParcel_ReceivesInvalidParcel_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => logic.SubmitParcel(invalidParcel));
        }

        [Test]
        public void SubmitParcel_ReceivesValidParcel_ReturnsTrackingIdOfParcel()
        {
            string expectedTrackingID = "PYJRB4HZ6";
            string actualTrackingID = logic.SubmitParcel(validParcel);

            Assert.AreEqual(expectedTrackingID, actualTrackingID);
        }

        [Test]
        public void TransitionParcel_ReceivesInvalidParcel_ThrowsException()
        {
            string trackingID = "PYJRB4HZ6";

            Assert.Throws<ArgumentOutOfRangeException>(() => logic.TransitionParcel(invalidParcel, trackingID));
        }

        [Test]
        public void TransitionParcel_ReceivesInvalidTrackingId_ThrowsException()
        {
            string trackingID = "ABCDEFGH";

            Assert.Throws<ArgumentOutOfRangeException>(() => logic.TransitionParcel(validParcel, trackingID));
        }

        [Test]
        public void TransitionParcel_ReceivesValidData_RunsWithoutException()
        {
            string trackingID = "PYJRB4HZ6";

            logic.TransitionParcel(validParcel, trackingID);

            Assert.IsTrue(true);
        }

    }
}