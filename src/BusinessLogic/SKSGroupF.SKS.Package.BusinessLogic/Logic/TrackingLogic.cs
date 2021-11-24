﻿using System;
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

namespace SKSGroupF.SKS.Package.BusinessLogic.Logic
{
    public class TrackingLogic : ITrackingLogic
    {
        private readonly IParcelRepository parcelRepo;
        private readonly IHopRepository hopRepo;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public TrackingLogic(IMapper mapper, IParcelRepository parcelRepo, IHopRepository hopRepo, ILogger<TrackingLogic> logger)
        {
            this.mapper = mapper;
            this.parcelRepo = parcelRepo;
            this.hopRepo = hopRepo;
            this.logger = logger;
        }

        public BLParcel TrackParcel(string trackingID)
        {
            BLParcel tmpParcel;

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
                    tmpParcel = mapper.Map<BLParcel>(parcelRepo.GetByTrackingId(trackingID));
                    return tmpParcel;
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

                    var tmpParcel = parcelRepo.GetByTrackingId(trackingID);
                    var hopArrival = mapper.Map<BLHopArrival>(parcelRepo.GetHopArrivalByCode(code, tmpParcel));
                    BLParcel parcel = mapper.Map<BLParcel>(tmpParcel);

                    parcel.FutureHops.Remove(hopArrival);
                    hopArrival.Visited = true;
                    parcel.VisitedHops.Add(hopArrival);

                    var hop = mapper.Map<BLHop>(hopRepo.GetByCode(code));

                    if (string.Compare(hop.HopType, "Warehouse") == 0)
                        parcel.State = BLParcel.StateEnum.InTransportEnum;
                    if (string.Compare(hop.HopType, "Truck") == 0)
                        parcel.State = BLParcel.StateEnum.InTruckDeliveryEnum;
                    if (string.Compare(hop.HopType, "TransferWarehouse") == 0)
                    {
                        //(POST https://<partnerUrl>/parcel/<trackingId>) 
                        parcel.State = BLParcel.StateEnum.TransferredEnum;
                    }

                    parcelRepo.Update(mapper.Map<DALParcel>(parcel));
                    //parcelRepo.UpdateHopState(parcelRepo.GetByTrackingId(trackingID), code);
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
