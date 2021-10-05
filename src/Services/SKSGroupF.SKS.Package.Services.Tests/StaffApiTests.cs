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
    public class StaffApiTests
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
        public void ReportParcelDelivery_BLGetsInvalidData_ReturnsErrorStatusCode()
        {
            Mock<ITrackingLogic> mockLogic = new();
            mockLogic.Setup(m => m.ReportParcelDelivery(It.IsNotIn("PYJRB4HZ6"))).Throws(new ArgumentOutOfRangeException());

            StaffApiController controller = new StaffApiController(mapper, mockLogic.Object);
            var result = (StatusCodeResult)controller.ReportParcelDelivery("ABCDEFGH");

            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void ReportParcelDelivery_BLGetsValidData_ReturnsOkStatusCode()
        {
            Mock<ITrackingLogic> mockLogic = new();
            mockLogic.Setup(m => m.ReportParcelDelivery(It.IsIn("PYJRB4HZ6")));

            StaffApiController controller = new StaffApiController(mapper, mockLogic.Object);
            var result = (StatusCodeResult)controller.ReportParcelDelivery("PYJRB4HZ6");

            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void ReportParcelHop_BLGetsInvalidDataTid_ReturnsErrorStatusCode()
        {
            Mock<ITrackingLogic> mockLogic = new();
            mockLogic.Setup(m => m.ReportParcelHop(It.IsNotIn("PYJRB4HZ6"), It.IsIn("ABCD\\aaaa"))).Throws(new ArgumentOutOfRangeException());

            StaffApiController controller = new StaffApiController(mapper, mockLogic.Object);
            var result = (StatusCodeResult)controller.ReportParcelHop("ABCDEFGH", "ABCD\\aaaa");

            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void ReportParcelHop_BLGetsInvalidDataCode_ReturnsErrorStatusCode()
        {
            Mock<ITrackingLogic> mockLogic = new();
            mockLogic.Setup(m => m.ReportParcelHop(It.IsIn("PYJRB4HZ6"), It.IsNotIn("ABCD\\aaaa"))).Throws(new ArgumentOutOfRangeException());

            StaffApiController controller = new StaffApiController(mapper, mockLogic.Object);
            var result = (StatusCodeResult)controller.ReportParcelHop("PYJRB4HZ6", "ABCD");

            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void ReportParcelHop_BLGetsInvalidDataTidCode_ReturnsErrorStatusCode()
        {
            Mock<ITrackingLogic> mockLogic = new();
            mockLogic.Setup(m => m.ReportParcelHop(It.IsNotIn("PYJRB4HZ6"), It.IsNotIn("ABCD\\aaaa"))).Throws(new ArgumentOutOfRangeException());

            StaffApiController controller = new StaffApiController(mapper, mockLogic.Object);
            var result = (StatusCodeResult)controller.ReportParcelHop("ABCDEFGH", "ABCD");

            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void ReportParcelHop_BLGetsValidDataCode_ReturnsOkayStatusCode()
        {
            Mock<ITrackingLogic> mockLogic = new();
            mockLogic.Setup(m => m.ReportParcelHop(It.IsIn("PYJRB4HZ6"), It.IsIn("ABCD\\aaaa")));

            StaffApiController controller = new StaffApiController(mapper, mockLogic.Object);
            var result = (StatusCodeResult)controller.ReportParcelHop("PYJRB4HZ6", "ABCD\\aaaa");

            Assert.AreEqual(200, result.StatusCode);
        }
    }
}