using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;

namespace SKSGroupF.SKS.Package.BusinessLogic
{
    public class TrackingLogic : ITrackingLogic
    {
        public string TrackParcel(string trackingID)
        {
            return "";
        }
        public void ReportParcelDelivery(string trackingID)
        {
            throw new NotImplementedException();
        }

        public void ReportParcelHop(string trackingID, string code)
        {
            throw new NotImplementedException();
        }
    }
}
