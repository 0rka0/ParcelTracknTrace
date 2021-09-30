using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SKSGroupF.SKS.Package.BusinessLogic.Entities.Models
{
    [ExcludeFromCodeCoverage]
    [DataContract]
    public class BLWarehouse
    {
        /// <summary>
        /// Gets or Sets Level
        /// </summary>
        [Required]

        [DataMember(Name = "level")]
        public int? Level { get; set; }

        /// <summary>
        /// Next hops after this warehouse (warehouses or trucks).
        /// </summary>
        /// <value>Next hops after this warehouse (warehouses or trucks).</value>
        [Required]

        [DataMember(Name = "nextHops")]
        public List<BLWarehouseNextHops> NextHops { get; set; }
    }
}
