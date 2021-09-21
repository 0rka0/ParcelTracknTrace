using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using SKSGroupF.SKS.Package.Services.Controllers;
using SKSGroupF.SKS.Package.Services.DTOs.Models;
using System;

namespace SKSGroupF.SKS.Package.Services.Test
{
    public class SenderApiTests
    {
        private SenderApiController controller;
        [SetUp]
        public void Setup()
        {
            this.controller = new SenderApiController();
        }

        [Test]
        public void InvalidData_ThrowsOutOfRangeException()
        {
            Parcel parcel = new Parcel();
            parcel.Receipient = null;
            parcel.Sender = new Receipient();
            parcel.Weight = 5.0f;

            Assert.Throws<ArgumentOutOfRangeException>(() => controller.SubmitParcel(parcel));
        }

        [Test]
        public void ValidData()
        {
            Parcel parcel = new Parcel();
            parcel.Receipient = new Receipient();
            parcel.Sender = new Receipient();
            parcel.Weight = 5.0f;

            ObjectResult result = (ObjectResult)controller.SubmitParcel(parcel);
            Assert.NotNull(result);
        }
    }
}