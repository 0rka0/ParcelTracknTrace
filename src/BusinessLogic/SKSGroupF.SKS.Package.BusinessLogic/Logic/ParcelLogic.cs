using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;

namespace SKSGroupF.SKS.Package.BusinessLogic.Logic
{
    public class ParcelLogic : IParcelLogic
    {
        public string SubmitParcel(BLParcel parcel)
        {
            return "PYJRB4HZ6";
        }

        public void TransitionParcel(BLParcel parcel, string trackingId)
        {
            if (String.Compare(trackingId, "PYJRB4HZ6") != 0)
                throw new ArgumentOutOfRangeException();
        }
    }
}
