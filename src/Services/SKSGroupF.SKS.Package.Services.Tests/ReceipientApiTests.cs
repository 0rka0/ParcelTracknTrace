using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using SKSGroupF.SKS.Package.Services.Controllers;
using SKSGroupF.SKS.Package.Services.DTOs.Models;
using System;
using AutoMapper;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using Moq;

namespace SKSGroupF.SKS.Package.Services.Test
{
    public class ReceipientApiTests
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
        public void TrackParcel_BLGetsInvalidData_ReturnsErrorStatusCode()
        {
            Mock<ITrackingLogic> mockLogic = new();
            mockLogic.Setup(m => m.TrackParcel(It.IsNotIn("PYJRB4HZ6"))).Throws(new ArgumentOutOfRangeException());

            ReceipientApiController controller = new ReceipientApiController(mapper, mockLogic.Object);
            var result = (ObjectResult)controller.TrackParcel("ABCDEFGH");

            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void TrackParcel_BLGets_ValidData_ReturnsNewBLParcelObjectWithState()
        {
            var mockBlParcel = new BusinessLogic.Entities.Models.BLParcel();
            mockBlParcel.State = BusinessLogic.Entities.Models.BLParcel.StateEnum.InTransportEnum;
            Mock<ITrackingLogic> mockLogic = new();
            mockLogic.Setup(m => m.TrackParcel(It.IsIn("PYJRB4HZ6"))).Returns(mockBlParcel);

            ReceipientApiController controller = new ReceipientApiController(mapper, mockLogic.Object);
            ObjectResult result = (ObjectResult)controller.TrackParcel("PYJRB4HZ6");

            Assert.NotNull(result.Value);
            Assert.AreEqual(TrackingInformation.StateEnum.InTransportEnum, ((TrackingInformation)result.Value).State);
        }
    }
}