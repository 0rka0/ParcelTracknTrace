using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Interfaces
{
    public interface IParcelLogic
    {
        public void SubmitParcel(/*Parcel parcel*/);
        public void TransitionParcel(/*Parcel body,*/ string trackingId);
    }
}
