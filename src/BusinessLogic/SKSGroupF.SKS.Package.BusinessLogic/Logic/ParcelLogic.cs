using System;
using System.Collections.Generic;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Validators;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using SKSGroupF.SKS.Package.ServiceAgents.Entities;
using SKSGroupF.SKS.Package.ServiceAgents.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces.Exceptions;

namespace SKSGroupF.SKS.Package.BusinessLogic.Logic
{
    public class ParcelLogic : IParcelLogic
    {
        private readonly IParcelRepository repo;
        private readonly IGeoEncodingAgent agent;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public ParcelLogic(IMapper mapper, IParcelRepository repo, IGeoEncodingAgent agent, ILogger<ParcelLogic> logger)
        {
            this.mapper = mapper;
            this.repo = repo;
            this.agent = agent;
            this.logger = logger;
        }

        public string SubmitParcel(BLParcel parcel)
        {
            parcel.TrackingId = "PYJRB4HZ6";
            parcel.FutureHops = new List<BLHopArrival>();
            parcel.VisitedHops = new List<BLHopArrival>();
            parcel.State = BLParcel.StateEnum.InTransportEnum;


            //var coor1 = mapper.Map<BLGeoCoordinate>(agent.EncodeAddress(mapper.Map<SAReceipient>(parcel.Receipient)));
            //var coor2 = mapper.Map<BLGeoCoordinate>(agent.EncodeAddress(mapper.Map<SAReceipient>(parcel.Sender)));

            IValidator<BLParcel> validator = new ParcelValidator();
            logger.LogDebug("Trying to validate submitted parcel.");
            var result = validator.Validate(parcel);

            try
            {
                if (result.IsValid)
                {
                    logger.LogInformation("Validation of parcel successful.");
                    try
                    {
                        logger.LogDebug("Trying to create parcel for database");
                        int? dbId = repo.Create(mapper.Map<DALParcel>(parcel));
              
                        logger.LogInformation("Parcel added to database successfully.");
                        return parcel.TrackingId;
              
                    }
                    catch (Exception ex)
                    {
                        string errorMsg = "Failed to create parcel for database.";
                        logger.LogError(errorMsg, ex);
                        throw new BLDataException(nameof(ParcelLogic), errorMsg);
                    }
                }
                else
                {
                    string errorMsg = "Failed to validate parcel.";
                    logger.LogError(errorMsg);
                    throw new BLValidationException(nameof(ParcelLogic), errorMsg);
                }
            }
            catch (Exception ex)
            {
                string errorMsg = "Error by submitting the parcel.";
                logger.LogError(errorMsg);
                throw new BLLogicException(nameof(ParcelLogic), errorMsg, ex);
            }
          
        }

        public void TransitionParcel(BLParcel parcel, string trackingId)
        {
            parcel.TrackingId = trackingId;

            IValidator<string> trackingIdValidator = new TrackingIdValidator();
            IValidator<BLParcel> parcelValidator = new ParcelValidator();

            logger.LogDebug("Trying to validate tracking Id.");
            var tidResult = trackingIdValidator.Validate(parcel.TrackingId);

            logger.LogDebug("Trying to validate submitted parcel.");
            var parcelResult = parcelValidator.Validate(parcel);


            try
            {
                if ((!tidResult.IsValid) || (!parcelResult.IsValid))
                {
                    string errorMsg = "Failed to validate tracking Id or parcel";
                    logger.LogError(errorMsg);
                    throw new BLValidationException(nameof(ParcelLogic), errorMsg);
                }

                logger.LogInformation("Validation successful.");
                try
                {
                    logger.LogDebug("Trying to update parcel in database.");
                    repo.Update(mapper.Map<DALParcel>(parcel));
                }
                catch
                {
                    string errorMsg = "Failed to update parcel in database.";
                    logger.LogError(errorMsg);
                    throw new BLDataException(nameof(ParcelLogic), errorMsg);
                }
            }
            catch (Exception ex)
            {
                string errorMsg = "Failed to transition the parcel.";
                logger.LogError(errorMsg);
                throw new BLLogicException(nameof(ParcelLogic), errorMsg, ex);
               
            }
        }
    }
}
