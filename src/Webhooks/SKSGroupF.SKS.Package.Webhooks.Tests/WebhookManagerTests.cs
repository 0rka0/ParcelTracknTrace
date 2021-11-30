using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using FizzWare.NBuilder;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using SKSGroupF.SKS.Package.DataAccess.Interfaces.Exceptions;
using SKSGroupF.SKS.Package.Webhooks;
using SKSGroupF.SKS.Package.Webhooks.Interfaces;
using SKSGroupF.SKS.Package.Webhooks.Interfaces.Exceptions;
using System;
using System.Net.Http;

namespace SKSGroupF.SKS.Package.Webhooks.Tests
{
    public class WebhookManagerTests
    {
        private IMapper mapper;
        IWebhookManager manager;
        Mock<IWebhookRepository> webhookRepo;
        MockHttpMessageHandler mockHttp;
        HttpClient client;
        DALWebhookResponse resp;
        DALWebhookResponses responses;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new WebhDalProfiles());
            });
            mapper = config.CreateMapper();

            resp = Builder<DALWebhookResponse>.CreateNew().With(d => d.Id = 1).Build();
            responses = new DALWebhookResponses();
            responses.Add(resp);
            responses.Add(Builder<DALWebhookResponse>.CreateNew().With(d => d.Id = 2).Build());
            responses.Add(Builder<DALWebhookResponse>.CreateNew().With(d => d.Id = 3).Build());

            mockHttp = new MockHttpMessageHandler();
            client = new HttpClient(mockHttp);

            webhookRepo = new();
        }

        [Test]
        public void Subscribe_SubscribesToParcel_ReturnsWebhookWithId()
        {
            webhookRepo.Setup(m => m.Create(It.IsAny<DALWebhookResponse>())).Returns(1);
            manager = new WebhookManager(mapper, webhookRepo.Object, client, new NullLogger<WebhookManager>());

            var resp = manager.Subscribe("ABCDEFGHI", "http://test.com");

            Assert.AreEqual(1, resp.Id);
        }

        [Test]
        public void Subscribe_SubscribesToParcelThatDoesNotExist_ThrowsNotFoundException()
        {
            webhookRepo.Setup(m => m.Create(It.IsAny<DALWebhookResponse>())).Throws(new DALDataNotFoundException(nameof(WebhookManagerTests), nameof(Subscribe_SubscribesToParcelThatDoesNotExist_ThrowsNotFoundException), "test"));
            manager = new WebhookManager(mapper, webhookRepo.Object, client, new NullLogger<WebhookManager>());

            Assert.Throws<WebhookLogicException>(() => manager.Subscribe("ABCDEFGHI", "http://test.com"));
        }

        [Test]
        public void Subscribe_UnknownErrorWhenSubscribing_ThrowsUnknownException()
        {
            webhookRepo.Setup(m => m.Create(It.IsAny<DALWebhookResponse>())).Throws(new Exception());
            manager = new WebhookManager(mapper, webhookRepo.Object, client, new NullLogger<WebhookManager>());

            Assert.Throws<WebhookLogicException>(() => manager.Subscribe("ABCDEFGHI", "http://test.com"));
        }

        [Test]
        public void Remove_RemovesSubscriptionById_SubscriptionRemoved()
        {
            webhookRepo.Setup(m => m.GetById(It.IsAny<long?>())).Returns(resp);
            webhookRepo.Setup(m => m.Delete(It.IsIn(1)));
            manager = new WebhookManager(mapper, webhookRepo.Object, client, new NullLogger<WebhookManager>());

            manager.Remove(1);

            Assert.IsTrue(true);
        }

        [Test]
        public void Remove_RemovesSubscriptionByIdThatDoesNotExist_ThrowsNotFoundException()
        {
            webhookRepo.Setup(m => m.GetById(It.IsAny<long?>())).Throws(new DALDataNotFoundException(nameof(WebhookManagerTests), nameof(Remove_RemovesSubscriptionById_SubscriptionRemoved), "test"));
            manager = new WebhookManager(mapper, webhookRepo.Object, client, new NullLogger<WebhookManager>());

            Assert.Throws<WebhookLogicException>(() => manager.Remove(1));
        }

        [Test]
        public void Remove_UnknownErrorWhenRemoving_ThrowsUnknownException()
        {
            webhookRepo.Setup(m => m.GetById(It.IsAny<long?>())).Returns(resp);
            webhookRepo.Setup(m => m.Delete(It.IsAny<long?>())).Throws(new Exception());
            manager = new WebhookManager(mapper, webhookRepo.Object, client, new NullLogger<WebhookManager>());

            Assert.Throws<WebhookLogicException>(() => manager.Remove(1));
        }

        [Test]
        public void AlertAll_AlertsAllSubscribers_SuccessfullyAlerted()
        {
            webhookRepo.Setup(m => m.GetAll()).Returns(responses);
            manager = new WebhookManager(mapper, webhookRepo.Object, client, new NullLogger<WebhookManager>());

            manager.AlertAll();

            Assert.IsTrue(true);
        }

        [Test]
        public void AlertAll_ErrorWhenAlertingAllSubscribers_ThrowsUnknownException()
        {
            webhookRepo.Setup(m => m.GetAll()).Throws(new Exception());
            manager = new WebhookManager(mapper, webhookRepo.Object, client, new NullLogger<WebhookManager>());

            Assert.Throws<AlertException>(() => manager.AlertAll());
        }

        [Test]
        public void AlertByTrackingId_AlertsAllSubscribersOfOneParcel_SuccessfullyAlerted()
        {
            webhookRepo.Setup(m => m.GetAllWithTrackingId(It.IsAny<string>())).Returns(responses);
            manager = new WebhookManager(mapper, webhookRepo.Object, client, new NullLogger<WebhookManager>());

            manager.AlertByTrackingId("ABCDEFH", "msg");

            Assert.IsTrue(true);
        }

        [Test]
        public void AlertByTrackingId_ErrorWhenAlertingAllSubscribersOfOneParcel_ThrowsUnknownException()
        {
            webhookRepo.Setup(m => m.GetAllWithTrackingId(It.IsAny<string>())).Throws(new Exception());
            manager = new WebhookManager(mapper, webhookRepo.Object, client, new NullLogger<WebhookManager>());

            Assert.Throws<AlertException>(() => manager.AlertByTrackingId("ABCDEFGHI", "msg"));
        }

        [Test]
        public void DeleteByTrackingId_DeletesAllSubscribersOfOneParcel_SuccessfullyDeleted()
        {
            webhookRepo.Setup(m => m.GetAllWithTrackingId(It.IsAny<string>())).Returns(responses);
            manager = new WebhookManager(mapper, webhookRepo.Object, client, new NullLogger<WebhookManager>());

            manager.DeleteByTrackingId("ABCDEFGHI");

            Assert.IsTrue(true);
        }

        [Test]
        public void DeleteByTrackingId_ErrorWhenDeletingAllSubscribersOfOneParcel_ThrowsUnknownException()
        {
            webhookRepo.Setup(m => m.GetAllWithTrackingId(It.IsAny<string>())).Throws(new Exception());
            manager = new WebhookManager(mapper, webhookRepo.Object, client, new NullLogger<WebhookManager>());

            Assert.Throws<WebhookLogicException>(() => manager.DeleteByTrackingId("ABCDEFGHI"));
        }
    }
}