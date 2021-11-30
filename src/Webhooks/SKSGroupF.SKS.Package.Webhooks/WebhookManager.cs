using AutoMapper;
using Microsoft.Extensions.Logging;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using SKSGroupF.SKS.Package.DataAccess.Interfaces.Exceptions;
using SKSGroupF.SKS.Package.Webhooks.Entities;
using SKSGroupF.SKS.Package.Webhooks.Interfaces;
using SKSGroupF.SKS.Package.Webhooks.Interfaces.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.Webhooks
{
    public class WebhookManager : IWebhookManager
    {
        private readonly IWebhookRepository webhookRepo;
        private readonly HttpClient client;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public WebhookManager(IMapper mapper, IWebhookRepository webhookRepo, HttpClient client, ILogger<WebhookManager> logger)
        {
            this.mapper = mapper;
            this.webhookRepo = webhookRepo;
            this.client = client;
            this.logger = logger;
        }

        public void AlertAll(string msgExtension = null)
        {
            string errorMsg;
            var message = "Your subscription has been deleted.";

            try
            {
                var webhooks = GetAllSubscriptions();

                foreach (var webhook in webhooks)
                {
                    Task task = Task.Run(async () => await Alert(webhook, message, msgExtension));
                }
            }
            catch (Exception ex)
            {
                errorMsg = "Error when alerting all subscriptions.";
                logger.LogError(errorMsg);
                throw new AlertException(nameof(WebhookManager), errorMsg, ex);
            }
        }

        public void AlertByTrackingId(string trackingId, string message, string msgExtension = null)
        {
            string errorMsg;

            try
            {
                var webhooks = GetSubscriptionsByTrackingId(trackingId);

                foreach (var webhook in webhooks)
                {
                    Task task = Task.Run(async () => await Alert(webhook, message, msgExtension));
                }
            }
            catch (Exception ex)
            {
                errorMsg = "Error when alerting all subscriptions for specified parcel.";
                logger.LogError(errorMsg);
                throw new AlertException(nameof(WebhookManager), errorMsg, ex);
            }
        }

        public void DeleteByTrackingId(string trackingId)
        {
            string errorMsg;

            try
            {
                var webhooks = GetSubscriptionsByTrackingId(trackingId);

                foreach (var webhook in webhooks)
                {
                    webhookRepo.Delete(webhook.Id);
                }
            }
            catch (Exception ex)
            {
                errorMsg = "Error when deleting all subscriptions for specified parcel.";
                logger.LogError(errorMsg);
                throw new WebhookLogicException(nameof(WebhookManager), errorMsg, ex);
            }
        }

        public async Task Alert(WebhookResponse webhook, string message, string msgExtension = null)
        {
            var values = new Dictionary<string, string>
            {
                { "Information", message },
                { "Tracking ID", webhook.TrackingId },
                { "Comment", msgExtension }
            };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(webhook.Url, content);
            var responseString = await response.Content.ReadAsStringAsync();
        }

        WebhookResponses GetAllSubscriptions()
        {
            string errorMsg;
            WebhookResponses webhooks = new WebhookResponses();

            try
            {
                foreach (var i in webhookRepo.GetAll())
                {
                    webhooks.Add(mapper.Map<WebhookResponse>(i));
                }

                return webhooks;
            }
            catch (Exception ex)
            {
                errorMsg = "Error when selecting subscriptions to parcel.";
                logger.LogError(errorMsg);
                throw new SubscriptionException(nameof(WebhookManager), errorMsg, ex);
            }
        }

        public WebhookResponses GetSubscriptionsByTrackingId(string trackingId)
        {
            string errorMsg;
            WebhookResponses webhooks = new WebhookResponses();

            try
            {
                foreach (var i in webhookRepo.GetAllWithTrackingId(trackingId))
                {
                    webhooks.Add(mapper.Map<WebhookResponse>(i));
                }

                return webhooks;
            }
            catch (Exception ex)
            {
                errorMsg = "Error when selecting subscriptions to parcel.";
                logger.LogError(errorMsg);
                throw new WebhookLogicException(nameof(WebhookManager), errorMsg, ex);
            }
        }

        public void Remove(long? id)
        {
            string errorMsg;

            try
            {
                var webhook = mapper.Map<WebhookResponse>(webhookRepo.GetById(id));
                webhookRepo.Delete(id);

                var message = "Your subscription has been deleted.";
                Task task = Task.Run(async () => await Alert(webhook, message));
            }
            catch (DALDataNotFoundException ex)
            {
                errorMsg = "Cannot delete parcel that does not exist in database.";
                logger.LogError(errorMsg);
                throw new WebhookLogicException(nameof(WebhookManager), errorMsg, ex);
            }
            catch (Exception ex)
            {
                errorMsg = "Error when subscribing to parcel.";
                logger.LogError(errorMsg);
                throw new WebhookLogicException(nameof(WebhookManager), errorMsg, ex);
            }
        }

        public WebhookResponse Subscribe(string trackingId, string url)
        {
            string errorMsg;
            WebhookResponse webhook = new WebhookResponse();
            webhook.Url = url;
            webhook.CreatedAt = DateTime.Now;
            webhook.TrackingId = trackingId;

            try
            {
                webhook.Id = webhookRepo.Create(mapper.Map<DALWebhookResponse>(webhook));

                var message = "You have successfully subscribed to this parcel.";
                Task task = Task.Run(async () => await Alert(webhook, message));

                return webhook;
            }
            catch(DALDataNotFoundException ex)
            {
                errorMsg = "Parcel to subscribe to does not exist in database.";
                logger.LogError(errorMsg);
                throw new WebhookLogicException(nameof(WebhookManager), errorMsg, ex);
            }
            catch (Exception ex)
            {
                errorMsg = "Error when subscribing to parcel.";
                logger.LogError(errorMsg);
                throw new WebhookLogicException(nameof(WebhookManager), errorMsg, ex);
            }
        }
    }
}
