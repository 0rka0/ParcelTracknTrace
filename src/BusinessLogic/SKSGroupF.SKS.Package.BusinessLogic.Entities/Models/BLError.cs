﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SKSGroupF.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    [DataContract]
    public class BLError
    {
        /// <summary>
        /// The error message.
        /// </summary>
        /// <value>The error message.</value>
        [Required]

        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
