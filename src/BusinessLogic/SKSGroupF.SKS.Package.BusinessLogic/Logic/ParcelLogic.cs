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
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;

namespace SKSGroupF.SKS.Package.BusinessLogic.Logic
{
    public class ParcelLogic : IParcelLogic
    {
        private readonly IParcelRepository repo;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public ParcelLogic(IMapper mapper, IParcelRepository repo, ILogger<ParcelLogic> logger)
        {
            this.mapper = mapper;
            this.repo = repo;
            this.logger = logger;
        }

        public string SubmitParcel(BLParcel parcel)
        {
            parcel.TrackingId = "PYJRB4HZ6";
            parcel.FutureHops = new List<BLHopArrival>();
            parcel.VisitedHops = new List<BLHopArrival>();
            parcel.State = BLParcel.StateEnum.InTransportEnum;

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

            IValidator<string> trackingIdValidator = new TrackingIdValidator();
            IValidator<BLParcel> parcelValidator = new ParcelValidator();

            var tidResult = trackingIdValidator.Validate(trackingId);
            var parcelResult = parcelValidator.Validate(parcel);

            if ((!tidResult.IsValid) || (!parcelResult.IsValid))
                throw new ArgumentOutOfRangeException();

            try
            {
                repo.Update(mapper.Map<DALParcel>(parcel));
            }
            catch
            {
                throw;
            }
        }
    }
}
