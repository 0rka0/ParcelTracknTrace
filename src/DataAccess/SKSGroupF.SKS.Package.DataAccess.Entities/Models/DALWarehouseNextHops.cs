using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SKSGroupF.SKS.Package.DataAccess.Entities.Models
{
    [ExcludeFromCodeCoverage]
    [DataContract]
    public class DALWarehouseNextHops
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [Required, Key]
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets TraveltimeMins
        /// </summary>
        [Required]
        [DataMember(Name = "traveltimeMins")]
        public int? TraveltimeMins { get; set; }

        /// <summary>
        /// Gets or Sets Hop
        /// </summary>
        [Required]
        [DataMember(Name = "hop")]
        public DALHop Hop { get; set; }
    }
}
