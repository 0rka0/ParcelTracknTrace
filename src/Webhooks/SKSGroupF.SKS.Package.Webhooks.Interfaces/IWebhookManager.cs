using SKSGroupF.SKS.Package.Webhooks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.Webhooks.Interfaces
{
    public interface IWebhookManager
    {
        WebhookResponses GetSubscriptionsByTrackingId(string trackingId);
        WebhookResponse Subscribe(string trackingId, string url);
        void Remove(long? id);
        void AlertAll(string msgExtension = null);
        void AlertByTrackingId(string trackingId, string message, string msgExtension = null);
        void DeleteByTrackingId(string trackingId);
    }
}
