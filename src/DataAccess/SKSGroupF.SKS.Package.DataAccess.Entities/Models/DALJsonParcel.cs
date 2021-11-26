using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.DataAccess.Entities.Models
{
    [ExcludeFromCodeCoverage]
    public class DALJsonParcel
    {

        public float? Weight { get; set; }

        public DALReceipient Receipient { get; set; }

        public DALReceipient Sender { get; set; }

        public DALJsonParcel(float? weight, DALReceipient rec, DALReceipient sender)
        {
            Weight = weight;
            Receipient = rec;
            Sender = sender;
        }
    }
}

