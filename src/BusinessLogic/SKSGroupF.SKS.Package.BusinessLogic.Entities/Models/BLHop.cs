using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SKSGroupF.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    [DataContract]
    public class BLHop
    {
        /// <summary>
        /// Gets or Sets HopType
        /// </summary>
        [Required]

        [DataMember(Name = "hopType")]
        public string HopType { get; set; }

        /// <summary>
        /// Unique CODE of the hop.
        /// </summary>
        /// <value>Unique CODE of the hop.</value>
        [Required]
        [RegularExpression("^[A-Z]{4}\\d{1,4}$")]
        [DataMember(Name = "code")]
        public string Code { get; set; }

        /// <summary>
        /// Description of the hop.
        /// </summary>
        /// <value>Description of the hop.</value>
        [Required]

        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Delay processing takes on the hop.
        /// </summary>
        /// <value>Delay processing takes on the hop.</value>
        [Required]

        [DataMember(Name = "processingDelayMins")]
        public int? ProcessingDelayMins { get; set; }

        /// <summary>
        /// Name of the location (village, city, ..) of the hop.
        /// </summary>
        /// <value>Name of the location (village, city, ..) of the hop.</value>
        [Required]

        [DataMember(Name = "locationName")]
        public string LocationName { get; set; }

        /// <summary>
        /// Gets or Sets LocationCoordinates
        /// </summary>
        [Required]

        [DataMember(Name = "locationCoordinates")]
        public BLGeoCoordinate LocationCoordinates { get; set; }

        /// <summary>
        /// The date/time the parcel arrived at the hop.
        /// </summary>
        /// <value>The date/time the parcel arrived at the hop.</value>
        [Required]

        [DataMember(Name = "dateTime")]
        public DateTime? DateTime { get; set; }
    }
}
