using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SKSGroupF.SKS.Package.DataAccess.Entities.Models
{
    [ExcludeFromCodeCoverage]
    [DataContract]
    public class DALParcel
    {
        /// <summary>
        /// Gets or Sets Weight
        /// </summary>
        [Required]

        [DataMember(Name = "weight")]
        public float? Weight { get; set; }

        /// <summary>
        /// Gets or Sets Receipient
        /// </summary>
        [Required]

        [DataMember(Name = "receipient")]
        public DALReceipient Receipient { get; set; }

        /// <summary>
        /// Gets or Sets Sender
        /// </summary>
        [Required]

        [DataMember(Name = "sender")]
        public DALReceipient Sender { get; set; }

        [RegularExpression("^[A-Z0-9]{9}$")]
        [DataMember(Name = "trackingId")]
        public string TrackingId { get; set; }

        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum StateEnum
        {
            /// <summary>
            /// Enum PickupEnum for Pickup
            /// </summary>
            [EnumMember(Value = "Pickup")]
            PickupEnum = 0,
            /// <summary>
            /// Enum InTransportEnum for InTransport
            /// </summary>
            [EnumMember(Value = "InTransport")]
            InTransportEnum = 1,
            /// <summary>
            /// Enum InTruckDeliveryEnum for InTruckDelivery
            /// </summary>
            [EnumMember(Value = "InTruckDelivery")]
            InTruckDeliveryEnum = 2,
            /// <summary>
            /// Enum TransferredEnum for Transferred
            /// </summary>
            [EnumMember(Value = "Transferred")]
            TransferredEnum = 3,
            /// <summary>
            /// Enum DeliveredEnum for Delivered
            /// </summary>
            [EnumMember(Value = "Delivered")]
            DeliveredEnum = 4
        }

        /// <summary>
        /// State of the parcel.
        /// </summary>
        /// <value>State of the parcel.</value>
        [Required]

        [DataMember(Name = "state")]
        public StateEnum? State { get; set; }

        /// <summary>
        /// Hops visited in the past.
        /// </summary>
        /// <value>Hops visited in the past.</value>
        [Required]

        [DataMember(Name = "visitedHops")]
        public List<DALHopArrival> VisitedHops { get; set; }

        /// <summary>
        /// Hops coming up in the future - their times are estimations.
        /// </summary>
        /// <value>Hops coming up in the future - their times are estimations.</value>
        [Required]

        [DataMember(Name = "futureHops")]
        public List<DALHopArrival> FutureHops { get; set; }
    }
}
