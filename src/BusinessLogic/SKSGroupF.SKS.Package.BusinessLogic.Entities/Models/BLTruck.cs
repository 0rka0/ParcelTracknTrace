using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SKSGroupF.SKS.Package.BusinessLogic.Entities.Models
{
    [ExcludeFromCodeCoverage]
    [DataContract]
    class BLTruck
    {
        /// <summary>
        /// GeoJSON of the are covered by the truck.
        /// </summary>
        /// <value>GeoJSON of the are covered by the truck.</value>
        [Required]

        [DataMember(Name = "regionGeoJson")]
        public string RegionGeoJson { get; set; }

        /// <summary>
        /// The truck&#x27;s number plate.
        /// </summary>
        /// <value>The truck&#x27;s number plate.</value>
        [Required]

        [DataMember(Name = "numberPlate")]
        public string NumberPlate { get; set; }
    }
}
