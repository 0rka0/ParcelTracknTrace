using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SKSGroupF.SKS.Package.DataAccess.Entities.Models
{
    [ExcludeFromCodeCoverage]
    [DataContract]
    public class DALGeoCoordinate
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [Required, Key]
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Latitude of the coordinate.
        /// </summary>
        /// <value>Latitude of the coordinate.</value>
        [Required]

        [DataMember(Name = "lat")]
        public double? Lat { get; set; }

        /// <summary>
        /// Longitude of the coordinate.
        /// </summary>
        /// <value>Longitude of the coordinate.</value>
        [Required]

        [DataMember(Name = "lon")]
        public double? Lon { get; set; }
    }
}
