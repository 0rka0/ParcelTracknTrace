using Newtonsoft.Json.Linq;
using SKSGroupF.SKS.Package.Services.DTOs.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SKSGroupF.SKS.Package.Services.DTOs
{
    [ExcludeFromCodeCoverage]
    public class HopJsonConverter : JsonCreationConverter<Hop>
    {
        protected override Hop Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            if (jObject["logisticsPartner"] != null)
            {
                return new Transferwarehouse();
            }
            else if (jObject["numberPlate"] != null)
            {
                return new Truck();
            }
            else
            {
                return new Warehouse();
            }
        }
    }
}
