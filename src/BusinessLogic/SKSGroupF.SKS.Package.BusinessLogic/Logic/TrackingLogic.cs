using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger logger;

        public TrackingLogic(IMapper mapper, IParcelRepository repo, ILogger<TrackingLogic> logger)
        {
            this.mapper = mapper;
            this.repo = repo;
            this.logger = logger;
        }

        public BLParcel TrackParcel(string trackingID)
        {
            BLParcel tmpParcel;

            logger.LogInformation("Validating tracking Id.");
            IValidator<string> validator = new TrackingIdValidator();
            var result = validator.Validate(trackingID);

            if (result.IsValid)
            {
                logger.LogInformation("Validation of tracking Id successful.");
                try
                {
                    logger.LogDebug("Trying to get parcel by tracking Id from database.");
                    tmpParcel = mapper.Map<BLParcel>(repo.GetByTrackingId(trackingID));
                    return tmpParcel;
                }
                catch
                {
                    logger.LogError("Failed to get parcel.");
                    throw;
                }
            }

            logger.LogError("Failed to validate tracking Id.");
            throw new ArgumentOutOfRangeException();
        }

        public void ReportParcelDelivery(string trackingID)
        {
            logger.LogInformation("Validating tracking Id.");
            IValidator<string> validator = new TrackingIdValidator();
            var result = validator.Validate(trackingID);

            if (!result.IsValid)
            {
                logger.LogError("Failed to validate tracking Id.");
                throw new ArgumentOutOfRangeException();
            }

            logger.LogInformation("Validation of tracking Id successful.");
            try
            {
                logger.LogDebug("Trying to update delivered-state of a parcel.");
                repo.UpdateDelivered(repo.GetByTrackingId(trackingID));
            }
            catch
            {
                logger.LogError("Failed to update parcel in database.");
                throw;
            }
        }

        public void ReportParcelHop(string trackingID, string code)
        {
            logger.LogInformation("Validating tracking Id.");
            IValidator<string> tidValidator = new TrackingIdValidator();
            logger.LogInformation("Validating code.");
            IValidator<string> codeValidator = new CodeValidator();

            var tidResult = tidValidator.Validate(trackingID);
            var codeResult = codeValidator.Validate(code);

            if ((!tidResult.IsValid) || (!codeResult.IsValid))
            {
                logger.LogError("Failed to validate tracking Id or Code");
                throw new ArgumentOutOfRangeException();
            }
            logger.LogInformation("Validation successful.");

            try
            {
                logger.LogDebug("Trying to update hop-state of a parcel.");
                repo.UpdateHopState(repo.GetByTrackingId(trackingID), code);
            }
            catch
            {
                logger.LogError("Failed to update parcel in database.");
                throw; 
            }
        }
    }
}
