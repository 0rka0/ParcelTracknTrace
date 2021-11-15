using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SKSGroupF.SKS.Package.ServiceAgents.Entities
{
    [ExcludeFromCodeCoverage]
    [DataContract]
    public class SAGeoCoordinate
    {
        /// <summary>
        /// Latitude of the coordinate.
        /// </summary>
        /// <value>Latitude of the coordinate.</value>
        [Required]

        [DataMember(Name = "lat")]
        public double? lat { get; set; }

        /// <summary>
        /// Longitude of the coordinate.
        /// </summary>
        /// <value>Longitude of the coordinate.</value>
        [Required]

        [DataMember(Name = "lon")]
        public double? lon { get; set; }
    }
}
