using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;

namespace SKSGroupF.SKS.Package.BusinessLogic
{
    public class TrackingLogic : ITrackingLogic
    {
        public BLParcel TrackParcel(string trackingID)
        {
            BLParcel tmpParcel = new BLParcel();

            if (String.Compare(trackingID, "PYJRB4HZ6") == 0)
                return tmpParcel;

            throw new ArgumentOutOfRangeException();
        }
        public void ReportParcelDelivery(string trackingID)
        {
            if (String.Compare(trackingID, "PYJRB4HZ6") != 0)
                throw new ArgumentOutOfRangeException();
        }

        public void ReportParcelHop(string trackingID, string code)
        {
            if ((String.Compare(trackingID, "PYJRB4HZ6") != 0) || (String.Compare(code, "PYJRB4HZ6") != 0))
                throw new ArgumentOutOfRangeException();
        }
    }
}
