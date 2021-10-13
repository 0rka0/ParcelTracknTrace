using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SKSGroupF.SKS.Package.DataAccess.Entities.Models
{
    [ExcludeFromCodeCoverage]
    [DataContract]
    public class DALWarehouse : DALHop
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
        public List<DALWarehouseNextHops> NextHops { get; set; }
    }
}
