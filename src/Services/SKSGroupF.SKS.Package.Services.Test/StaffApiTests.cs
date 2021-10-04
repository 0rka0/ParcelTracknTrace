using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using SKSGroupF.SKS.Package.Services.Controllers;
using SKSGroupF.SKS.Package.Services.DTOs.Models;
using System;
using AutoMapper;

namespace SKSGroupF.SKS.Package.Services.Test
{
    public class StaffApiTests
    {
        private StaffApiController controller;
        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new SvcBlProfiles());
            });
            var mapper = config.CreateMapper();
            this.controller = new StaffApiController(mapper);
        }

        [Test]
        public void ReportParcelDelivery_InvalidData_ThrowsOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => controller.ReportParcelDelivery("ABCDEFGH"));
        }

        [Test]
        public void ReportParcelDelivery_ValidData()
        {
            Assert.Throws<NotImplementedException>(() => controller.ReportParcelDelivery("PYJRB4HZ6"));
        }

        [Test]
        public void ReportParcelHop_InvalidDataTrackingId_ThrowsOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => controller.ReportParcelHop("ABCDEFGH", "ABCD\\dddd"));
        }

        [Test]
        public void ReportParcelHop_InvalidDataCode_ThrowsOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => controller.ReportParcelHop("ABCDEFGH", "ABCD\\aaaa"));
        }

        [Test]
        public void ReportParcelHop_ValidData()
        {
            Assert.Throws<NotImplementedException>(() => controller.ReportParcelHop("PYJRB4HZ6", "ABCD\\dddd"));
        }
    }
}