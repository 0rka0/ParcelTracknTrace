using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using SKSGroupF.SKS.Package.Services.Controllers;
using SKSGroupF.SKS.Package.Services.DTOs.Models;
using System;
using AutoMapper;

namespace SKSGroupF.SKS.Package.Services.Test
{
    public class ReceipientApiTests
    {
        private ReceipientApiController controller;
        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new SvcBlProfiles());
            });
            var mapper = config.CreateMapper();
            this.controller = new ReceipientApiController(mapper);
        }

        [Test]
        public void InvalidData_ThrowsOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => controller.TrackParcel("ABCDEFGH"));
        }

        [Test]
        public void ValidData()
        {
            ObjectResult result = (ObjectResult)controller.TrackParcel("PYJRB4HZ6");
            Assert.NotNull(result);
        }
    }
}