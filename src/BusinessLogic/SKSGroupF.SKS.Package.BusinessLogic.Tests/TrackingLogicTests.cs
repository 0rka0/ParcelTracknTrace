using AutoMapper;
using Moq;
using NUnit.Framework;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Logic;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using System;

namespace SKSGroupF.SKS.Package.BusinessLogic.Tests
{
    class TrackingLogicTests
    {
        private ITrackingLogic logic;
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {
            DALParcel parcel = new DALParcel();
            parcel.TrackingId = "PYJRB4HZ6";

            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new BlDalProfiles());
            });
            mapper = config.CreateMapper();

            Mock<IParcelRepository> mockRepo = new();
            mockRepo.Setup(m => m.GetByTrackingId(It.IsAny<string>())).Returns(parcel);
            mockRepo.Setup(m => m.UpdateHopState(It.IsAny<DataAccess.Entities.Models.DALParcel>(), It.IsAny<string>()));

            logic = new TrackingLogic(mapper, mockRepo.Object);
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
        public void ReportParcelHop_ReceivesValidData_RunsWithoutException()
        {
            logic.ReportParcelHop("PYJRB4HZ6", "ABCD\\dddd");

            Assert.IsTrue(true);
        }
    }
}
