using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Interfaces
{
    public interface ITrackingLogic
    {
        public string TrackParcel(string trackingID);
        public void ReportParcelDelivery(string trackingID);
        public void ReportParcelHop(string trackingID, string code);
    }
}
