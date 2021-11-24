using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SKSGroupF.SKS.Package.BusinessLogic.Entities.Models
{
    [ExcludeFromCodeCoverage]
    [DataContract]
    public class BLHopArrival
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
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

        public BLParcel DALParcelId { get; set; }
        public BLParcel DALParcelId1 { get; set; }

        public bool Visited { get; set; } = false;

        /// <summary>
        /// The date/time the parcel arrived at the hop.
        /// </summary>
        /// <value>The date/time the parcel arrived at the hop.</value>
        [Required]

        [DataMember(Name = "dateTime")]
        public DateTime? DateTime { get; set; }
    }
}
