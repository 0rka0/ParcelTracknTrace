using AutoMapper;
using Moq;
using NUnit.Framework;
using FizzWare.NBuilder;
using SKSGroupF.SKS.Package.Webhooks.Entities;
using SKSGroupF.SKS.Package.Services.DTOs.Models;
using SKSGroupF.SKS.Package.Webhooks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKSGroupF.SKS.Package.Services.Controllers;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SKSGroupF.SKS.Package.Webhooks.Interfaces.Exceptions;

namespace SKSGroupF.SKS.Package.Services.Test
{
    public class ParcelWebhookApiTests
    {
        private IMapper mapper;
        private Mock<IWebhookManager> mockManager;
        private Webhooks.Entities.WebhookResponses webhooks;
        Webhooks.Entities.WebhookResponse webhook;

        DateTime time = DateTime.Now;

        [SetUp]
        public void Setup()
        {

            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new SvcWebhProfiles());
            });
            mapper = config.CreateMapper();

            mockManager = new();

            webhook = Builder<Webhooks.Entities.WebhookResponse>.CreateNew()
                .With(p => p.TrackingId = "ABCDEFGHI")
                .With(p => p.CreatedAt = time)
                .With(p => p.Url = "http://test.com")
                .Build();

            webhooks = new Webhooks.Entities.WebhookResponses();
            webhooks.Add(webhook);
            webhooks.Add(Builder<Webhooks.Entities.WebhookResponse>.CreateNew().Build());
        }

        [Test]
        public void ApiParcelByTrackingIdWebhooksGet_GetsValidTrackingId_ReturnsOkStatusCode()
        {
            mockManager.Setup(m => m.GetSubscriptionsByTrackingId(It.IsAny<string>())).Returns(webhooks);

            ParcelWebhookApiController controller = new ParcelWebhookApiController(mapper, mockManager.Object, new NullLogger<ParcelWebhookApiController>());
            var result = (ObjectResult)controller.ApiParcelByTrackingIdWebhooksGet("ABCDEFGHI");

            Assert.NotNull(result.Value);
            Assert.AreEqual(2, ((Services.DTOs.Models.WebhookResponses)result.Value).Count);
        }

        [Test]
        public void ApiParcelByTrackingIdWebhooksGet_GetsInvalidTrackingId_ReturnsErrorStatusCode()
        {
            mockManager.Setup(m => m.GetSubscriptionsByTrackingId(It.IsNotIn("ABCDEF123"))).Throws(new WebhookLogicException(nameof(ApiParcelByTrackingIdWebhooksGet_GetsInvalidTrackingId_ReturnsErrorStatusCode), "test", new Exception()));

            ParcelWebhookApiController controller = new ParcelWebhookApiController(mapper, mockManager.Object, new NullLogger<ParcelWebhookApiController>());
            var result = (ObjectResult)controller.ApiParcelByTrackingIdWebhooksGet("ABCDEFGHI");

            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void ApiParcelByTrackingIdWebhooksPost_GetsValidData_ReturnsOkStatusCode()
        {
            mockManager.Setup(m => m.Subscribe(It.IsAny<string>(), It.IsAny<string>())).Returns(webhooks[0]);

            ParcelWebhookApiController controller = new ParcelWebhookApiController(mapper, mockManager.Object, new NullLogger<ParcelWebhookApiController>());
            var result = (ObjectResult)controller.ApiParcelByTrackingIdWebhooksPost("ABCDEFGHI", "http://test.com");

            Assert.NotNull(result.Value);
            Assert.AreEqual("ABCDEFGHI", ((Services.DTOs.Models.WebhookResponse)result.Value).TrackingId);
            Assert.AreEqual(time, ((Services.DTOs.Models.WebhookResponse)result.Value).CreatedAt);
        }

        [Test]
        public void ApiParcelByTrackingIdWebhooksPost_GetsInvalidData_ReturnsErrorStatusCode()
        {
            mockManager.Setup(m => m.Subscribe(It.IsNotIn("ABCDEF123"), It.IsAny<string>())).Throws(new WebhookLogicException(nameof(ApiParcelByTrackingIdWebhooksGet_GetsInvalidTrackingId_ReturnsErrorStatusCode), "test", new Exception()));

            ParcelWebhookApiController controller = new ParcelWebhookApiController(mapper, mockManager.Object, new NullLogger<ParcelWebhookApiController>());
            var result = (ObjectResult)controller.ApiParcelByTrackingIdWebhooksPost("ABCDEFGHI", "http://test.com");

            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void ApiParcelWebhooksByIdDelete_GetsValidData_ReturnsOkStatusCode()
        {
            mockManager.Setup(m => m.Remove(It.IsAny<long?>()));

            ParcelWebhookApiController controller = new ParcelWebhookApiController(mapper, mockManager.Object, new NullLogger<ParcelWebhookApiController>());
            var result = (StatusCodeResult)controller.ApiParcelWebhooksByIdDelete(5);

            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void ApiParcelWebhooksByIdDelete_GetsInvalidData_ReturnsErrorStatusCode()
        {
            mockManager.Setup(m => m.Remove(It.IsAny<long?>())).Throws(new WebhookLogicException(nameof(ApiParcelWebhooksByIdDelete_GetsInvalidData_ReturnsErrorStatusCode), "test", new Exception()));

            ParcelWebhookApiController controller = new ParcelWebhookApiController(mapper, mockManager.Object, new NullLogger<ParcelWebhookApiController>());
            var result = (ObjectResult)controller.ApiParcelWebhooksByIdDelete(5);

            Assert.AreEqual(404, result.StatusCode);
        }
    }
}
