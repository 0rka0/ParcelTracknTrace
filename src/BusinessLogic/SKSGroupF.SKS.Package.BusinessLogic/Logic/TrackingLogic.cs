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
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using SKSGroupF.SKS.Package.BusinessLogic.Validators;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using SKSGroupF.SKS.Package.DataAccess.Interfaces.Exceptions;
using SKSGroupF.SKS.Package.Webhooks.Interfaces;

namespace SKSGroupF.SKS.Package.BusinessLogic.Logic
{
    public class TrackingLogic : ITrackingLogic
    {
        private readonly IParcelRepository parcelRepo;
        private readonly IHopRepository hopRepo;
        private readonly IWebhookManager webhookManager;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public TrackingLogic(IMapper mapper, IParcelRepository parcelRepo, IHopRepository hopRepo, IWebhookManager webhookManager, ILogger<TrackingLogic> logger)
        {
            this.mapper = mapper;
            this.parcelRepo = parcelRepo;
            this.hopRepo = hopRepo;
            this.webhookManager = webhookManager;
            this.logger = logger;
        }

        public BLParcel TrackParcel(string trackingID)
        {
            BLParcel parcel;

            logger.LogDebug("Trying to validate tracking Id.");
            IValidator<string> validator = new TrackingIdValidator();
            var result = validator.Validate(trackingID);

            try
            {
                if (!result.IsValid)
                {
                    string errorMsg = "Failed to validate tracking Id.";
                    logger.LogError(errorMsg);
                    throw new BLValidationException(nameof(TrackingLogic), errorMsg);
                }

                logger.LogInformation("Validation of tracking Id successful.");
                try
                {
                    logger.LogDebug("Trying to get parcel by tracking Id from database.");
                    var tmpParcel = parcelRepo.GetByTrackingId(trackingID);
                    parcel = mapper.Map<BLParcel>(tmpParcel);
                    foreach (var ha in parcelRepo.GetHopArrivalsByParcel(tmpParcel, false))
                        parcel.FutureHops.Add(mapper.Map<BLHopArrival>(ha));
                    foreach (var ha in parcelRepo.GetHopArrivalsByParcel(tmpParcel, true))
                        parcel.VisitedHops.Add(mapper.Map<BLHopArrival>(ha));

                    return parcel;
                }
                catch (DALDataNotFoundException ex)
                {
                    string errorMsg = "Failed to get the parcel by tracking id.";
                    logger.LogError(errorMsg);
                    throw new BLDataNotFoundException(nameof(TrackingLogic), errorMsg, ex);
                }
                catch (Exception ex)
                {
                    string errorMsg = "Failed to get parcel.";
                    logger.LogError(errorMsg);
                    throw new BLDataException(nameof(TrackingLogic), errorMsg, ex);
                }

            }
            catch (Exception ex)
            {
                string errorMsg = "Failed to track the parcel.";
                logger.LogError(errorMsg);
                throw new BLLogicException(nameof(TrackingLogic), errorMsg, ex);
            }
        }

        public void ReportParcelDelivery(string trackingID)
        {
            logger.LogDebug("Trying to validate tracking Id.");
            IValidator<string> validator = new TrackingIdValidator();
            var result = validator.Validate(trackingID);
            logger.LogDebug("Trying to report parcel delivery.");

            try
            {

                if (!result.IsValid)
                {
                    string errorMsg = "Failed to validate tracking Id.";
                    logger.LogError(errorMsg);
                    throw new BLValidationException(nameof(TrackingLogic), errorMsg);
                }

                logger.LogInformation("Validation of tracking Id successful.");
                try
                {
                    logger.LogDebug("Trying to update delivered-state of a parcel.");
                    parcelRepo.UpdateDelivered(parcelRepo.GetByTrackingId(trackingID));

                    var message = "Your subscription has been deleted.";
                    var msgExtension = "The parcel has reached its destination and the subscription has therefore gotten removed.";
                    webhookManager.AlertByTrackingId(trackingID, message, msgExtension);
                    webhookManager.DeleteByTrackingId(trackingID);
                }
                catch (Exception ex)
                {
                    string errorMsg = "Failed to update parcel in database.";
                    logger.LogError(errorMsg);
                    throw new BLDataException(nameof(TrackingLogic), errorMsg, ex);
                }
            }
            catch (Exception ex)
            {
                string errorMsg = "Failed to report parcel delivery.";
                logger.LogError(errorMsg);
                throw new BLLogicException(nameof(TrackingLogic), errorMsg, ex);
            }
        }

        public void ReportParcelHop(string trackingID, string code)
        {
            logger.LogDebug("Trying to validate tracking Id.");
            IValidator<string> tidValidator = new TrackingIdValidator();
            logger.LogDebug("Trying to validate code.");
            IValidator<string> codeValidator = new CodeValidator();

            var tidResult = tidValidator.Validate(trackingID);
            var codeResult = codeValidator.Validate(code);

            logger.LogDebug("Trying to report parcel hop.");
            try
            {

                if ((!tidResult.IsValid) || (!codeResult.IsValid))
                {
                    string errorMsg = "Failed to validate tracking Id or Code";
                    logger.LogError(errorMsg);
                    throw new BLValidationException(nameof(TrackingLogic), errorMsg);
                }
                logger.LogInformation("Validation successful.");

                try
                {
                    logger.LogDebug("Trying to update hop-state of a parcel.");

                    var hop = hopRepo.GetByCode(code);
                    parcelRepo.UpdateHopState(trackingID, code, hop);

                    var message = "State of subscripted parcel has been changed.";
                    var msgExtension = "The parcel has reached the " + hop.HopType + ": " + hop.Code + " - " + hop.Description + ".\nCheck for further information.";
                    webhookManager.AlertByTrackingId(trackingID, message, msgExtension);
                }
                catch (Exception ex)
                {
                    string errorMsg = "Failed to update parcel in database.";
                    logger.LogError(errorMsg);
                    throw new BLDataException(nameof(TrackingLogic), errorMsg, ex);
                }
            }
            catch (Exception ex)
            {
                string errorMsg = "Failed to report parcel hop.";
                logger.LogError(errorMsg);
                throw new BLLogicException(nameof(TrackingLogic), errorMsg, ex);
            }
        }
    }
}
