/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace SKSGroupF.SKS.Package.Services.DTOs.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage][DataContract]
    public partial class Receipient
    { 
        /// <summary>
        /// Name of person or company.
        /// </summary>
        /// <value>Name of person or company.</value>
        [Required]

        [DataMember(Name="name")]
        public string Name { get; set; }

        /// <summary>
        /// Street
        /// </summary>
        /// <value>Street</value>
        [Required]

        [DataMember(Name="street")]
        public string Street { get; set; }

        /// <summary>
        /// Postalcode
        /// </summary>
        /// <value>Postalcode</value>
        [Required]

        [DataMember(Name="postalCode")]
        public string PostalCode { get; set; }

        /// <summary>
        /// City
        /// </summary>
        /// <value>City</value>
        [Required]

        [DataMember(Name="city")]
        public string City { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        /// <value>Country</value>
        [Required]

        [DataMember(Name="country")]
        public string Country { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Receipient {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Street: ").Append(Street).Append("\n");
            sb.Append("  PostalCode: ").Append(PostalCode).Append("\n");
            sb.Append("  City: ").Append(City).Append("\n");
            sb.Append("  Country: ").Append(Country).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Receipient)obj);
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (Name != null)
                    hashCode = hashCode * 59 + Name.GetHashCode();
                    if (Street != null)
                    hashCode = hashCode * 59 + Street.GetHashCode();
                    if (PostalCode != null)
                    hashCode = hashCode * 59 + PostalCode.GetHashCode();
                    if (City != null)
                    hashCode = hashCode * 59 + City.GetHashCode();
                    if (Country != null)
                    hashCode = hashCode * 59 + Country.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Receipient left, Receipient right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Receipient left, Receipient right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
