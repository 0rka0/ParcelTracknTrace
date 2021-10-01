using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Interfaces
{
    public interface ITrackingLogic
    {
        public BLParcel TrackParcel(string trackingID);
        public void ReportParcelDelivery(string trackingID);
        public void ReportParcelHop(string trackingID, string code);
    }
}
