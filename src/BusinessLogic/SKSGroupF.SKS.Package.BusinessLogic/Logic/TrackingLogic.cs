using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Validators;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;

namespace SKSGroupF.SKS.Package.BusinessLogic.Logic
{
    public class TrackingLogic : ITrackingLogic
    {
        private readonly IParcelRepository repo;
        private readonly IMapper mapper;

        public TrackingLogic(IMapper mapper, IParcelRepository repo)
        {
            this.mapper = mapper;
            this.repo = repo;
        }

        public BLParcel TrackParcel(string trackingID)
        {
            BLParcel tmpParcel;

            IValidator<string> validator = new StringValidator(true);
            var result = validator.Validate(trackingID);

            if (result.IsValid)
            {
                try
                {
                    tmpParcel = mapper.Map<BLParcel>(repo.GetByTrackingId(trackingID));
                    return tmpParcel;
                }
                catch
                {
                    throw;
                }
            }

            throw new ArgumentOutOfRangeException();
        }

        public void ReportParcelDelivery(string trackingID)
        {
            IValidator<string> validator = new StringValidator(true);
            var result = validator.Validate(trackingID);

            if (!result.IsValid)
                throw new ArgumentOutOfRangeException();

            try
            {
                repo.UpdateDelivered();
            }
            catch
            {
                throw;
            }
        }

        public void ReportParcelHop(string trackingID, string code)
        {
            IValidator<string> tidValidator = new StringValidator(true);
            IValidator<string> codeValidator = new StringValidator(false);

            var tidResult = tidValidator.Validate(trackingID);
            var codeResult = codeValidator.Validate(code);

            if ((!tidResult.IsValid) || (!codeResult.IsValid))
                throw new ArgumentOutOfRangeException();

            try
            {
                repo.UpdateHopState(repo.GetByTrackingId(trackingID), code);
            }
            catch
            {
                throw; 
            }
        }
    }
}
