using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SKSGroupF.SKS.Package.Webhooks.Entities
{
    public class WebhookResponse
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>

        [DataMember(Name = "id")]
        public long? Id { get; set; }

        /// <summary>
        /// Gets or Sets TrackingId
        /// </summary>
        [RegularExpression("^[A-Z0-9]{9}$")]
        [DataMember(Name = "trackingId")]
        public string TrackingId { get; set; }

        /// <summary>
        /// Gets or Sets Url
        /// </summary>

        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or Sets CreatedAt
        /// </summary>

        [DataMember(Name = "created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}
