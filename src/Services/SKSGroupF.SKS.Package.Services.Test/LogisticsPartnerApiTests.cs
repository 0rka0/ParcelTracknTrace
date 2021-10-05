using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using SKSGroupF.SKS.Package.Services.Controllers;
using SKSGroupF.SKS.Package.Services.DTOs.Models;
using System;
using AutoMapper;
using Moq;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using FizzWare.NBuilder;

namespace SKSGroupF.SKS.Package.Services.Test
{
    public class LogisticsPartnerApiTests
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
        public void TransitionParcel_BLGetsInvalidDataTid_ReturnsErrorStatusCode()
        {
            Mock<IParcelLogic> mockLogic = new();
            mockLogic.Setup(m => m.TransitionParcel(It.IsAny<BusinessLogic.Entities.Models.BLParcel>(), It.IsNotIn("PYJRB4HZ6"))).Throws(new ArgumentOutOfRangeException());

            LogisticsPartnerApiController controller = new LogisticsPartnerApiController(mapper, mockLogic.Object);

            var parcel = Builder<Parcel>.CreateNew()
                .With(p => p.Receipient = Builder<Receipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<Receipient>.CreateNew().Build())
                .With(p => p.Weight = 0)
                .Build();

            var result = (ObjectResult)controller.TransitionParcel(parcel, "ABCDEFGH");

            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public void TransitionParcel_BLGetsInvalidDataParcel_ReturnsErrorStatusCode()
        {
            Mock<IParcelLogic> mockLogic = new();
            mockLogic.Setup(m => m.TransitionParcel(It.IsAny<BusinessLogic.Entities.Models.BLParcel>(), It.IsIn("PYJRB4HZ6"))).Throws(new ArgumentOutOfRangeException());

            LogisticsPartnerApiController controller = new LogisticsPartnerApiController(mapper, mockLogic.Object);

            var parcel = Builder<Parcel>.CreateNew()
                .With(p => p.Receipient = Builder<Receipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<Receipient>.CreateNew().Build())
                .With(p => p.Weight = 0)
                .Build();

            var result = (ObjectResult)controller.TransitionParcel(parcel, "PYJRB4HZ6");

            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public void TransitionParcel_BLGetsInvalidDataTidParcel_ReturnsErrorStatusCode()
        {
            Mock<IParcelLogic> mockLogic = new();
            mockLogic.Setup(m => m.TransitionParcel(It.IsAny<BusinessLogic.Entities.Models.BLParcel>(), It.IsNotIn("PYJRB4HZ6"))).Throws(new ArgumentOutOfRangeException());

            LogisticsPartnerApiController controller = new LogisticsPartnerApiController(mapper, mockLogic.Object);

            var parcel = Builder<Parcel>.CreateNew()
                .With(p => p.Receipient = Builder<Receipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<Receipient>.CreateNew().Build())
                .With(p => p.Weight = 0)
                .Build();

            var result = (ObjectResult)controller.TransitionParcel(parcel, "ABCDEFGH");

            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public void ValidData()
        {
            Mock<IParcelLogic> mockLogic = new();
            mockLogic.Setup(m => m.TransitionParcel(It.IsAny<BusinessLogic.Entities.Models.BLParcel>(), It.IsIn("PYJRB4HZ6")));

            LogisticsPartnerApiController controller = new LogisticsPartnerApiController(mapper, mockLogic.Object);

            var parcel = Builder<Parcel>.CreateNew()
                .With(p => p.Receipient = Builder<Receipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<Receipient>.CreateNew().Build())
                .With(p => p.Weight = 0)
                .Build();

            var result = (ObjectResult)controller.TransitionParcel(parcel, "PYJRB4HZ6");

            Assert.NotNull(result.Value);
            Assert.AreEqual("PYJRB4HZ6", ((NewParcelInfo)result.Value).TrackingId);
        }
    }
}