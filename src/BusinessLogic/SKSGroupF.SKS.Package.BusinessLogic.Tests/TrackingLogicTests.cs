using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using SKSGroupF.SKS.Package.BusinessLogic.Logic;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using SKSGroupF.SKS.Package.DataAccess.Interfaces.Exceptions;
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

            logic = new TrackingLogic(mapper, mockRepo.Object, new NullLogger<TrackingLogic>());
        }

        [Test]
        public void TrackParcel_ReceivesInvalidTrackingId_ThrowsException()
        {
            Assert.Throws<BLLogicException>(() => logic.TrackParcel("ABCDEFGH"));
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
        public void TrackParcel_ReceivesTrackingId_ThrowsUnknownException()
        {
            Mock<IParcelRepository> mockRepo = new();
            mockRepo.Setup(m => m.GetByTrackingId(It.IsAny<string>())).Throws(new Exception());
            logic = new TrackingLogic(mapper, mockRepo.Object, new NullLogger<TrackingLogic>());

            Assert.Throws<BLLogicException>(() => logic.TrackParcel("PYJRB4HZ6"));
        }

        [Test]
        public void TrackParcel_ReceivesTrackingId_ThrowsDataNotFoundException()
        {
            Mock<IParcelRepository> mockRepo = new();
            mockRepo.Setup(m => m.GetByTrackingId(It.IsAny<string>())).Throws(new DALDataNotFoundException(nameof(TrackingLogicTests), nameof(TrackParcel_ReceivesTrackingId_ThrowsDataNotFoundException), "test"));
            logic = new TrackingLogic(mapper, mockRepo.Object, new NullLogger<TrackingLogic>());

            Assert.Throws<BLLogicException>(() => logic.TrackParcel("PYJRB4HZ6"));
        }

        [Test]
        public void ReportParcelDelivery_ReceivesInvalidTrackingId_ThrowsLogicException()
        {
            Assert.Throws<BLLogicException>(() => logic.ReportParcelDelivery("ABCDEFGH"));
        }

        [Test]
        public void ReportParcelDelivery_ReceivesValidTrackingId_RunsWithoutException()
        {
            logic.ReportParcelDelivery("PYJRB4HZ6");

            Assert.IsTrue(true);
        }

        [Test]
        public void ReportParcelHop_ReceivesInvalidTrackingId_ThrowsLogicException()
        {
            Assert.Throws<BLLogicException>(() => logic.ReportParcelHop("ABCDEFGH", "ABCD\\dddd"));
        }

        [Test]
        public void ReportParcelHop_ReceivesInvalidCode_ThrowsLogicException()
        {
            Assert.Throws<BLLogicException>(() => logic.ReportParcelHop("PYJRB4HZ6", "ABCD\\ddddd"));
        }

        [Test]
        public void ReportParcelHop_ReceivesValidData_RunsWithoutException()
        {
            logic.ReportParcelHop("PYJRB4HZ6", "ABCD\\dddd");

            Assert.IsTrue(true);
        }
    }
}
