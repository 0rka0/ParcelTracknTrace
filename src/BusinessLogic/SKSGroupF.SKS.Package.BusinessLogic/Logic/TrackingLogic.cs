using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Validators;

namespace SKSGroupF.SKS.Package.BusinessLogic.Logic
{
    public class TrackingLogic : ITrackingLogic
    {
        public BLParcel TrackParcel(string trackingID)
        {
            BLParcel tmpParcel = new BLParcel();
            tmpParcel.TrackingId = trackingID;

            IValidator<string> validator = new StringValidator(true);
            var result = validator.Validate(tmpParcel.TrackingId);

            if (result.IsValid)
                return tmpParcel;

            throw new ArgumentOutOfRangeException();
        }

        public void ReportParcelDelivery(string trackingID)
        {
            IValidator<string> validator = new StringValidator(true);
            var result = validator.Validate(trackingID);

            if (!result.IsValid)
                throw new ArgumentOutOfRangeException();
        }

        public void ReportParcelHop(string trackingID, string code)
        {
            IValidator<string> tidValidator = new StringValidator(true);
            IValidator<string> codeValidator = new StringValidator(false);

            var tidResult = tidValidator.Validate(trackingID);
            var codeResult = codeValidator.Validate(code);

            if ((!tidResult.IsValid) || (!codeResult.IsValid))
                throw new ArgumentOutOfRangeException();
        }
    }
}
