using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SKSGroupF.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    [DataContract]
    class BLWarehouseNextHops
    {
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
        public BLHop Hop { get; set; }
    }
}
