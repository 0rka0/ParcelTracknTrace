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

            var coor1 = mapper.Map<BLGeoCoordinate>(agent.EncodeAddress(mapper.Map<SAReceipient>(parcel.Receipient)));
            var coor2 = mapper.Map<BLGeoCoordinate>(agent.EncodeAddress(mapper.Map<SAReceipient>(parcel.Sender)));

            logger.LogInformation("Validating submitted parcel.");
            IValidator<BLParcel> validator = new ParcelValidator();

            var result = validator.Validate(parcel);

            if (result.IsValid)
            {
                logger.LogInformation("Validation of parcel successful.");
                try
                {
                    logger.LogDebug("Trying to create parcel for database");
                    int? dbId = repo.Create(mapper.Map<DALParcel>(parcel));

                    if (dbId != null)
                    {
                        logger.LogInformation("Parcel added to database successfully.");
                        return parcel.TrackingId;
                    }
                }
                catch
                {
                    logger.LogError("Failed to create parcel for database.");
                    throw;
                }
            }

            logger.LogError("Failed to validate parcel.");
            throw new ArgumentOutOfRangeException();
        }

        public void TransitionParcel(BLParcel parcel, string trackingId)
        {
            parcel.TrackingId = trackingId;

            logger.LogInformation("Validating tracking Id.");
            IValidator<string> trackingIdValidator = new TrackingIdValidator();
            logger.LogInformation("Validating submitted parcel.");
            IValidator<BLParcel> parcelValidator = new ParcelValidator();

            var tidResult = trackingIdValidator.Validate(parcel.TrackingId);
            var parcelResult = parcelValidator.Validate(parcel);

            if ((!tidResult.IsValid) || (!parcelResult.IsValid))
            {
                logger.LogError("Failed to validate tracking Id or parcel");
                throw new ArgumentOutOfRangeException();
            }
            logger.LogInformation("Validation successful.");

            try
            {
                logger.LogDebug("Trying to update parcel in database.");
                repo.Update(mapper.Map<DALParcel>(parcel));
            }
            catch
            {
                logger.LogError("Failed to update parcel in database.");
                throw;
            }
        }
    }
}
