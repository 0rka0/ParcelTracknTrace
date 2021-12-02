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

            parcel = PredictRoute(parcel);

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
            logger.LogDebug("Trying to predict route between sender and recipient.");
            
            parcel.FutureHops = new List<BLHopArrival>();
            parcel.VisitedHops = new List<BLHopArrival>();

            try
            {
                var coorRec = mapper.Map<BLGeoCoordinate>(agent.EncodeAddress(mapper.Map<SAReceipient>(parcel.Receipient)));
                var coorSender = mapper.Map<BLGeoCoordinate>(agent.EncodeAddress(mapper.Map<SAReceipient>(parcel.Sender)));

                var hops = GetAllTrucksAndTWhs();
                var reader = new GeoJsonReader();

                var hopRec = GetHopByCoor(reader, hops, new Point((double)coorRec.Lon, (double)coorRec.Lat));
                var hopSender = GetHopByCoor(reader, hops, new Point((double)coorSender.Lon, (double)coorSender.Lat));

                if (hopRec == null || hopSender == null)
                {
                    string errorMsg = "Could not find truck for either sender or receipient.";
                    logger.LogError(errorMsg);
                    throw new BLDataNotFoundException(nameof(ParcelLogic), errorMsg);
                }

                parcel.FutureHops = GetRouteFromHop(hopRec, hopSender);

                return parcel;
            }
            catch (Exception ex)
            {
                string errorMsg = "An error occurred when trying to predict route between sender and recipient.";
                logger.LogError(errorMsg);
                throw new BLLogicException(nameof(ParcelLogic), errorMsg, ex);
            }
        }

        private List<BLHop> GetAllTrucksAndTWhs()
        {
            try
            {
                logger.LogDebug("Trying to get all trucks and transfer warehouses from repository.");

                var tmpHopList = hopRepo.GetAll();
                List<BLHop> truckList = new List<BLHop>();
                foreach (var i in tmpHopList)
                {
                    if (String.Compare(i.HopType, "Truck") == 0)
                        truckList.Add(mapper.Map<BLHop>(i));
                    if (String.Compare(i.HopType, "TransferWarehouse") == 0)
                        truckList.Add(mapper.Map<BLHop>(i));
                }
                return truckList;

            }
            catch (Exception ex)
            {
                string errorMsg = "An error occurred when getting all trucks and transfer warehouses from repository.";
                logger.LogError(errorMsg);
                throw new BLLogicException(nameof(ParcelLogic), errorMsg, ex);
            }

        }

        private Geometry GetGeometry(GeoJsonReader reader, BLHop hop)
        {
            try
            {
                logger.LogDebug("Trying to create a geometry.");
                Feature g;

                if (String.Compare(hop.HopType, "Truck") == 0)
                    g = reader.Read<Feature>(((BLTruck)hop).RegionGeoJson);
                else
                    g = reader.Read<Feature>(((BLTransferWarehouse)hop).RegionGeoJson);

                if (!Orientation.IsCCW(g.Geometry.Coordinates))
                    g.Geometry = g.Geometry.Reverse();
                return g.Geometry;
            }
            catch (Exception ex)
            {
                string errorMsg = "An error occurred when creating a geometry.";
                logger.LogError(errorMsg);
                throw new BLLogicException(nameof(ParcelLogic), errorMsg, ex);
            }
        }

        private BLHop GetHopByCoor(GeoJsonReader reader, List<BLHop> hops, Point point)
        {
            try
            {
                logger.LogDebug("Trying to get a truck by his coordinates.");
                foreach (var truck in hops)
                {
                    Geometry region = GetGeometry(reader, truck);
                    if (region.Contains(point))
                        return truck;
                }
                return null;
            }
            catch (Exception ex)
            {
                string errorMsg = "An error occurred when getting a truck by its coordinates.";
                logger.LogError(errorMsg);
                throw new BLLogicException(nameof(ParcelLogic), errorMsg, ex);
            }
        }

        private List<BLHopArrival> GetRouteFromHop(BLHop hopRec, BLHop hopSender)
        {
            logger.LogDebug("Trying to get route from trucks.");

            List<BLHop> recList = new List<BLHop>();
            List<BLHop> senderList = new List<BLHop>();
            recList.Add(hopRec);
            senderList.Add(hopSender);
            try
            {
                while (true)
                {
                    hopRec = hopRec.Parent;
                    hopSender = hopSender.Parent;

                    if (hopRec.Code == hopSender.Code)
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
            catch (Exception ex)
            {
                string errorMsg = "An error occurred when trying to get route from trucks.";
                logger.LogError(errorMsg);
                throw new BLLogicException(nameof(ParcelLogic), errorMsg, ex);
            }
        }
    }
}
