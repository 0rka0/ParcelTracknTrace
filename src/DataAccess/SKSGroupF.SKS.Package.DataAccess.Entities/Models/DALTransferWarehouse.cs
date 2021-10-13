using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SKSGroupF.SKS.Package.DataAccess.Entities.Models
{
    [ExcludeFromCodeCoverage]
    [DataContract]
    public class DALTransferWarehouse : DALHop
    {
        /// <summary>
        /// GeoJSON of the are covered by the logistics partner.
        /// </summary>
        /// <value>GeoJSON of the are covered by the logistics partner.</value>
        [Required]

        [DataMember(Name = "regionGeoJson")]
        public string RegionGeoJson { get; set; }

        /// <summary>
        /// Name of the logistics partner.
        /// </summary>
        /// <value>Name of the logistics partner.</value>
        [Required]

        [DataMember(Name = "logisticsPartner")]
        public string LogisticsPartner { get; set; }

        /// <summary>
        /// BaseURL of the logistics partner&#x27;s REST service.
        /// </summary>
        /// <value>BaseURL of the logistics partner&#x27;s REST service.</value>
        [Required]

        [DataMember(Name = "logisticsPartnerUrl")]
        public string LogisticsPartnerUrl { get; set; }
    }
}
