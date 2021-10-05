using NUnit.Framework;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Logic;
using System;

namespace SKSGroupF.SKS.Package.BusinessLogic.Tests
{
    class TrackingLogicTests
    {
        private ITrackingLogic logic;

        [SetUp]
        public void Setup()
        {
            logic = new TrackingLogic();
        }

        [Test]
        public void TrackParcel_ReceivesInvalidTrackingId_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => logic.TrackParcel("ABCDEFGH"));
        }

        [Test]
        public void TrackParcel_ReceivesValidTrackingId_ReturnsBLParcelObjectWithTrackingId()
        {
            string trackingId = "PYJRB4HZ6";
            var parcel = logic.TrackParcel(trackingId);

            Assert.IsNotNull(parcel);
            Assert.AreEqual(trackingId, parcel.TrackingId);
        }

        [Test]
        public void ReportParcelDelivery_ReceivesInvalidTrackingId_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => logic.ReportParcelDelivery("ABCDEFGH"));
        }

        [Test]
        public void ReportParcelDelivery_ReceivesValidTrackingId_RunsWithoutException()
        {
            logic.ReportParcelDelivery("PYJRB4HZ6");

            Assert.IsTrue(true);
        }

        [Test]
        public void ReportParcelHop_ReceivesInvalidTrackingId_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => logic.ReportParcelHop("ABCDEFGH", "ABCD\\dddd"));
        }

        [Test]
        public void ReportParcelHop_ReceivesInvalidCode_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => logic.ReportParcelHop("PYJRB4HZ6", "ABCD\\ddddd"));
        }

        [Test]
        public void ReportParcelHop_eceivesValidData_RunsWithoutException()
        {
            logic.ReportParcelHop("PYJRB4HZ6", "ABCD\\dddd");

            Assert.IsTrue(true);
        }
    }
}
