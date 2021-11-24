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
using Fare;
using SKSGroupF.SKS.Package.DataAccess.Interfaces.Exceptions;
using NetTopologySuite.IO;
using NetTopologySuite.Features;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;

namespace SKSGroupF.SKS.Package.BusinessLogic.Logic
{
    public class ParcelLogic : IParcelLogic
    {
        private readonly IParcelRepository parcelRepo;
        private readonly IHopRepository hopRepo;
        private readonly IGeoEncodingAgent agent;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public ParcelLogic(IMapper mapper, IParcelRepository parcelRepo, IHopRepository hopRepo, IGeoEncodingAgent agent, ILogger<ParcelLogic> logger)
        {
            this.mapper = mapper;
            this.parcelRepo = parcelRepo;
            this.hopRepo = hopRepo;
            this.agent = agent;
            this.logger = logger;
        }

        public string SubmitParcel(BLParcel parcel)
        {
            string errorMsg;
            bool retry;
            var trackingIdGenerator = new Xeger("^[A-Z0-9]{9}$");
            parcel.State = BLParcel.StateEnum.PickupEnum;

            parcel = PredictRoute(parcel);

            do
            {
                parcel.TrackingId = trackingIdGenerator.Generate();

                IValidator<BLParcel> validator = new ParcelValidator();
                logger.LogDebug("Trying to validate submitted parcel.");
                var result = validator.Validate(parcel);

                try
                {
                    if (!result.IsValid)
                    {
                        errorMsg = "Failed to validate parcel.";
                        logger.LogError(errorMsg);
                        throw new BLValidationException(nameof(ParcelLogic), errorMsg);
                    }

                    logger.LogInformation("Validation of parcel successful.");
                    try
                    {
                        logger.LogDebug("Trying to create parcel for database");
                        int? dbId = parcelRepo.Create(mapper.Map<DALParcel>(parcel));

                        logger.LogInformation("Parcel added to database successfully.");
                        retry = false;
                        return parcel.TrackingId;

                    }
                    catch (DALDataDuplicateException)
                    {
                        logger.LogWarning("Duplicate tracking Id - Retrying insertion process.");
                        retry = true;
                    }
                    catch (Exception ex)
                    {
                        errorMsg = "Failed to create parcel for database.";
                        logger.LogError(errorMsg);
                        throw new BLDataException(nameof(ParcelLogic), errorMsg, ex);
                    }
                }
                catch (Exception ex)
                {
                    errorMsg = "Error by submitting the parcel.";
                    logger.LogError(errorMsg);
                    throw new BLLogicException(nameof(ParcelLogic), errorMsg, ex);
                }
            } while (retry);
            errorMsg = "Unknown error by submitting the parcel.";
            logger.LogError(errorMsg);
            throw new BLLogicException(nameof(ParcelLogic), errorMsg);
        }

        public void TransitionParcel(BLParcel parcel, string trackingId)
        {
            string errorMsg;
            parcel.State = BLParcel.StateEnum.PickupEnum;
            parcel.TrackingId = trackingId;

            IValidator<string> trackingIdValidator = new TrackingIdValidator();
            IValidator<BLParcel> parcelValidator = new ParcelValidator();

            logger.LogDebug("Trying to validate tracking Id.");
            var tidResult = trackingIdValidator.Validate(parcel.TrackingId);

            logger.LogDebug("Trying to validate submitted parcel.");
            var parcelResult = parcelValidator.Validate(parcel);

            parcel = PredictRoute(parcel);

            try
            {
                if ((!tidResult.IsValid) || (!parcelResult.IsValid))
                {
                    errorMsg = "Failed to validate tracking Id or parcel";
                    logger.LogError(errorMsg);
                    throw new BLValidationException(nameof(ParcelLogic), errorMsg);
                }

                logger.LogInformation("Validation successful.");
                try
                {
                    logger.LogDebug("Trying to create parcel for database");
                    int? dbId = parcelRepo.Create(mapper.Map<DALParcel>(parcel));

                    logger.LogInformation("Parcel added to database successfully.");
                }
                catch (DALDataDuplicateException ex)
                {
                    errorMsg = "Tracking id is already in use.";
                    logger.LogError(errorMsg);
                    throw new BLDataDuplicateException(nameof(ParcelLogic), errorMsg, ex);
                }
                catch (Exception ex)
                {
                    errorMsg = "Failed to create parcel for database.";
                    logger.LogError(errorMsg);
                    throw new BLDataException(nameof(ParcelLogic), errorMsg, ex);
                }
            }
            catch (Exception ex)
            {
                errorMsg = "Failed to transition the parcel.";
                logger.LogError(errorMsg);
                throw new BLLogicException(nameof(ParcelLogic), errorMsg, ex);       
            }
        }

        private BLParcel PredictRoute(BLParcel parcel)
        {
            parcel.FutureHops = new List<BLHopArrival>();
            parcel.VisitedHops = new List<BLHopArrival>();
            var coorRec = mapper.Map<BLGeoCoordinate>(agent.EncodeAddress(mapper.Map<SAReceipient>(parcel.Receipient)));
            var coorSender = mapper.Map<BLGeoCoordinate>(agent.EncodeAddress(mapper.Map<SAReceipient>(parcel.Sender)));

            var trucks = GetAllTrucks();
            var reader = new GeoJsonReader();

            var truckRec = GetTruckByCoor(reader, trucks, new Point((double)coorRec.Lon, (double)coorRec.Lat));
            var truckSender = GetTruckByCoor(reader, trucks, new Point((double)coorSender.Lon, (double)coorSender.Lat));

            if (truckRec == null || truckSender == null)
            {
                string errorMsg = "Could not find truck for either sender or receipient.";
                logger.LogError(errorMsg);
                throw new BLDataNotFoundException(nameof(ParcelLogic), errorMsg);
            }

            parcel.FutureHops = GetRouteFromTrucks(truckRec, truckSender);

            return parcel;
        }

        private List<BLTruck> GetAllTrucks()
        {
            var tmpHopList = hopRepo.GetAll();

            List<BLTruck> truckList = new List<BLTruck>();
            foreach (var i in tmpHopList)
            {
                if (String.Compare(i.HopType, "Truck") == 0)
                    truckList.Add((BLTruck)mapper.Map<BLHop>(i));
            }

            return truckList;
        }

        private Geometry GetGeometry(GeoJsonReader reader, BLTruck truck)
        {
            Feature g = reader.Read<Feature>(truck.RegionGeoJson);
            if (!Orientation.IsCCW(g.Geometry.Coordinates))
                g.Geometry = g.Geometry.Reverse();
            return g.Geometry;
        }

        private BLTruck GetTruckByCoor(GeoJsonReader reader, List<BLTruck> trucks, Point point)
        {
            foreach (var truck in trucks)
            {
                Geometry region = GetGeometry(reader, truck);
                if (region.Contains(point))
                    return truck;
            }
            return null;
        }

        private List<BLHopArrival> GetRouteFromTrucks(BLHop hopRec, BLHop hopSender)
        {
            List<BLHop> recList = new List<BLHop>();
            List<BLHop> senderList = new List<BLHop>();
            recList.Add(hopRec);
            senderList.Add(hopSender);
            while (true)
            {
                hopRec = hopRec.Parent;
                hopSender = hopSender.Parent;

                if(hopRec.Code == hopSender.Code)
                {
                    senderList.Add(hopRec);
                    break;
                }

                recList.Add(hopRec);
                senderList.Add(hopSender);
            }

            recList.Reverse();
            foreach (var hop in recList)
                senderList.Add(hop);

            List<BLHopArrival> res = new List<BLHopArrival>();
            foreach (var hop in senderList)
            {
                var tmp = mapper.Map<BLHopArrival>(hop);
                res.Add(tmp);
            }

            return res;
        }
    }
}
