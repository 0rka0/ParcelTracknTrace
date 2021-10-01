using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Interfaces
{
    public interface IParcelLogic
    {
        public void SubmitParcel(BLParcel parcel);
        public void TransitionParcel(BLParcel body, string trackingId);
    }
}
