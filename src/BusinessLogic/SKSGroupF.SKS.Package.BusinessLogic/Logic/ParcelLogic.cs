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
    public class ParcelLogic : IParcelLogic
    {
        public string SubmitParcel(BLParcel parcel)
        {
            parcel.TrackingId = "PYJRB4HZ6";

            IValidator<BLParcel> validator = new ParcelValidator();

            var result = validator.Validate(parcel);

            if(result.IsValid)
                return parcel.TrackingId;

            throw new ArgumentOutOfRangeException();
        }

        public void TransitionParcel(BLParcel parcel, string trackingId)
        {
            parcel.TrackingId = trackingId;

            IValidator<string> trackingIdValidator = new StringValidator(true);
            IValidator<BLParcel> parcelValidator = new ParcelValidator();

            var tidResult = trackingIdValidator.Validate(trackingId);
            var parcelResult = parcelValidator.Validate(parcel);

            if ((!tidResult.IsValid) || (!parcelResult.IsValid))
            {
                throw new ArgumentOutOfRangeException(
                    string.Concat(tidResult.Errors.Select(p=>p.ErrorMessage)) + "---" + string.Concat(parcelResult.Errors.Select(p => p.ErrorMessage))
                    );
            }
        }
    }
}
