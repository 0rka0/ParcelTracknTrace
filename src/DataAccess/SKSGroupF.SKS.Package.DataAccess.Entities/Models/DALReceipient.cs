using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SKSGroupF.SKS.Package.DataAccess.Entities.Models
{
    [ExcludeFromCodeCoverage]
    [DataContract]
    public class DALReceipient
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [Required, Key]
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Name of person or company.
        /// </summary>
        /// <value>Name of person or company.</value>
        [Required]

        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Street
        /// </summary>
        /// <value>Street</value>
        [Required]

        [DataMember(Name = "street")]
        public string Street { get; set; }

        /// <summary>
        /// Postalcode
        /// </summary>
        /// <value>Postalcode</value>
        [Required]

        [DataMember(Name = "postalCode")]
        public string PostalCode { get; set; }

        /// <summary>
        /// City
        /// </summary>
        /// <value>City</value>
        [Required]

        [DataMember(Name = "city")]
        public string City { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        /// <value>Country</value>
        [Required]

        [DataMember(Name = "country")]
        public string Country { get; set; }
    }
}
