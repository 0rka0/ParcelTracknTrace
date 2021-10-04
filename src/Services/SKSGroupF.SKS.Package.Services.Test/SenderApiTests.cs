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
    public class SenderApiTests
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
        public void SubmitParcel_InvalidData_ReturnsErrorStatusCode()
        {
            Mock<IParcelLogic> mockLogic = new();
            mockLogic.Setup(m => m.SubmitParcel(It.IsAny<BusinessLogic.Entities.Models.BLParcel>())).Throws(new ArgumentOutOfRangeException());

            SenderApiController controller = new SenderApiController(mapper, mockLogic.Object);

            var parcel = Builder<Parcel>.CreateNew()
                .With(p => p.Receipient = Builder<Receipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<Receipient>.CreateNew().Build())
                .With(p => p.Weight = 0)
                .Build();

            var result = (ObjectResult)controller.SubmitParcel(parcel);


            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public void SubmitParcel_ValidData_ReturnsNewParcelInfoObjectWithTrackingId()
        {
            Mock<IParcelLogic> mockLogic = new();
            mockLogic.Setup(m => m.SubmitParcel(It.IsAny<BusinessLogic.Entities.Models.BLParcel>())).Returns("PYJRB4HZ6");

            SenderApiController controller = new SenderApiController(mapper, mockLogic.Object);

            var parcel = Builder<Parcel>.CreateNew()
                .With(p => p.Receipient = Builder<Receipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<Receipient>.CreateNew().Build())
                .With(p => p.Weight = 0)
                .Build();

            ObjectResult result = (ObjectResult)controller.SubmitParcel(parcel);
            Assert.NotNull(result.Value);
            Assert.AreEqual("PYJRB4HZ6", ((NewParcelInfo)result.Value).TrackingId);
        }
    }
}