using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using SKSGroupF.SKS.Package.Services.Controllers;
using SKSGroupF.SKS.Package.Services.DTOs.Models;
using System;

namespace SKSGroupF.SKS.Package.Services.Test
{
    public class LogisticsPartnerApiTests
    {
        private LogisticsPartnerApiController controller;
        [SetUp]
        public void Setup()
        {
            this.controller = new LogisticsPartnerApiController();
        }

        [Test]
        public void InvalidDataTrackingId_ThrowsOutOfRangeException()
        {
            Parcel parcel = new Parcel();
            parcel.Sender = new Receipient();
            parcel.Receipient = new Receipient();
            parcel.Weight = 5.0f;
            Assert.Throws<ArgumentOutOfRangeException>(() => controller.TransitionParcel(parcel, "ABCDEFGH"));
        }

        [Test]
        public void InvalidDataParcel_ThrowsOutOfRangeException()
        {
            Parcel parcel = new Parcel();
            parcel.Sender = new Receipient();
            parcel.Receipient = null;
            parcel.Weight = 5.0f;
            Assert.Throws<ArgumentOutOfRangeException>(() => controller.TransitionParcel(parcel, "PYJRB4HZ6"));
        }

        [Test]
        public void ValidData()
        {
            Parcel parcel = new Parcel();
            parcel.Sender = new Receipient();
            parcel.Receipient = new Receipient();
            parcel.Weight = 5.0f;
            ObjectResult result = (ObjectResult)controller.TransitionParcel(parcel, "PYJRB4HZ6");
            Assert.NotNull(result);
        }
    }
}