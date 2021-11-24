using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using SKSGroupF.SKS.Package.DataAccess.Interfaces.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKSGroupF.SKS.Package.DataAccess.Sql
{
    public class SqlParcelRepository : IParcelRepository
    {
        private ISqlDbContext context;
        private readonly ILogger logger;

        public SqlParcelRepository(ISqlDbContext context, ILogger<SqlParcelRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public int Create(DALParcel parcel)
        {
            string errorMsg;
            try
            {
                GetByTrackingId(parcel.TrackingId);
            }
            catch (DALDataNotFoundException) //Parcel will only be created if GetByTrackingId does not find a parcel with this tracking Id in the database
            {
                try
                {
                    //Insert Parcel and return db ID
                    logger.LogInformation("Inserting parcel into database.");
                    var ent = context.DbParcel.Add(parcel);
                    logger.LogInformation("Parcel inserted with ID + " + ent.Entity.Id + ".");
                    SaveChanges();
                    return ent.Entity.Id;
                }
                catch (Exception ex)
                {
                    errorMsg = "There has been an error with the database";
                    logger.LogError(errorMsg, ex);
                    throw new DALDataException(nameof(SqlParcelRepository), nameof(Create), errorMsg, ex);
                }
            }

            errorMsg = "Parcel with specified tracking Id does already exsist in database.";
            logger.LogError(errorMsg);
            throw new DALDataDuplicateException(nameof(SqlParcelRepository), nameof(Create), errorMsg);         
        }

        public void Update(DALParcel parcel)
        {
            logger.LogInformation("Updating existing parcel in database with tracking Id " + parcel.TrackingId + ".");
            context.DbParcel.Update(parcel);
            SaveChanges();
        }

        public void Delete(int id)
        {
            logger.LogInformation("Removing existing parcel from database.");
            var parcel = context.DbParcel.Find(id);
            if(parcel == null)
            {
                string errorMsg = "Parcel with specified Id could not be found in database.";
                logger.LogError(errorMsg);
                throw new DALDataNotFoundException(nameof(SqlParcelRepository), nameof(Delete), errorMsg);
            }
            context.DbParcel.Remove(parcel);
            SaveChanges();
        }

        public void Clear()
        {
            logger.LogInformation("Clearing all parcels from database.");

            foreach (var rec in context.DbReceipient)
                context.DbReceipient.Remove(rec);

            foreach (var hopArrival in context.DbHopArrival)
                context.DbHopArrival.Remove(hopArrival);

            foreach (var parcel in context.DbParcel)
                context.DbParcel.Remove(parcel);
            SaveChanges();
        }

        public IEnumerable<DALParcel> GetAll()
        {
            try
            {
                logger.LogInformation("Trying to select all parcels from database.");
                return context.DbParcel.ToList();
            }
            catch (Exception ex)
            {
                string errorMsg = "No parcels found in Database.";
                logger.LogError(errorMsg, ex);
                throw new DALDataNotFoundException(nameof(SqlParcelRepository), nameof(GetAll), errorMsg, ex);
            }
        }

        public DALParcel GetByTrackingId(string tid)
        {
            try
            {
                logger.LogInformation("Trying to select a single parcel with tracking Id " + tid + ".");
                var parcels = context.DbParcel.Include(a => a.Receipient).Include(a => a.Sender).ToList();
                return parcels.Single(p => p.TrackingId == tid);
            }
            catch (Exception ex)
            {
                string errorMsg = "Parcel with specified tracking Id could not be found in Database.";
                logger.LogError(errorMsg, ex);
                throw new DALDataNotFoundException(nameof(SqlHopRepository), nameof(GetByTrackingId), errorMsg, ex);
            }
        }

        public DALHopArrival GetHopArrivalByCode(string code, DALParcel id)
        {
            try
            {
                logger.LogInformation("Trying to select a single hopArrival with code " + code + ".");
                var tmp = context.DbHopArrival.Include(a => a.FhopsId).Include(a => a.VhopsId);
                return tmp.Single(p => p.Code == code && !p.Visited && p.FhopsId == id);
            }
            catch (Exception ex)
            {
                string errorMsg = "HopArrival with specified code could not be found in Database.";
                logger.LogError(errorMsg, ex);
                throw new DALDataNotFoundException(nameof(SqlHopRepository), nameof(GetHopArrivalByCode), errorMsg, ex);
            }
        }

        public DALHopArrival GetHopArrivalByParcel(DALParcel parcel, bool visited)
        {
            try
            {
                logger.LogInformation("Trying to select a single hopArrival by parcel - visited: + " + visited + ".");
                return context.DbHopArrival.Single(p => p.Visited);
            }
            catch (Exception ex)
            {
                string errorMsg = "HopArrival with specified code could not be found in Database.";
                logger.LogError(errorMsg, ex);
                throw new DALDataNotFoundException(nameof(SqlHopRepository), nameof(GetHopArrivalByCode), errorMsg, ex);
            }
        }

        /*public IEnumerable<DALParcel> GetByReceipient(DALReceipient receipient)
        {
            try
            {
                return context.DbParcel.Where(p => p.Receipient == receipient).ToList();
            }
            catch { return null; }
        }

        public IEnumerable<DALParcel> GetBySender(DALReceipient sender)
        {
            try
            {
                return context.DbParcel.Where(p => p.Sender == sender).ToList();
            }
            catch { return null; }
        }

        public IEnumerable<DALParcel> GetByState(DALParcel.StateEnum state)
        {
            try
            {
                return context.DbParcel.Where(p => p.State == state).ToList();
            }
            catch { return null; }
        }

        public IEnumerable<DALParcel> GetByWeight(float min, float max)
        {
            try
            {
                return context.DbParcel.Where(p => p.Weight >= min && p.Weight <= max).ToList();
            }
            catch { return null; }
        }*/

        /*public void UpdateHopState(DALParcel parcel, string code)
        {
            try
            {
                logger.LogInformation("Changing hop-state of parcel.");
                //?
                var hop = context.DbHopArrival.Single(p => p.Code == code);
                parcel.FutureHops.Remove(hop);
                parcel.VisitedHops.Add(hop);
                context.DbParcel.Update(parcel);
                SaveChanges();
            }
            catch (Exception ex)
            {
                string errorMsg = "Could not update hop-state.";
                logger.LogError(errorMsg);
                throw new DALDataException(nameof(SqlHopRepository), nameof(GetAll), errorMsg, ex);
            }
            //Selects FutureHop by Code and marks it as VisitedHop
        }*/

        public void UpdateDelivered(DALParcel parcel)
        {
            try
            {
                logger.LogInformation("Changing delivered-state of parcel.");
                parcel.State = DALParcel.StateEnum.DeliveredEnum;
                Update(parcel);
            }
            catch (Exception ex)
            {
                string errorMsg = "Could not update delivered-state state.";
                logger.LogError(errorMsg);
                throw new DALDataException(nameof(SqlHopRepository), nameof(GetAll), errorMsg, ex);
            }
        }

        int SaveChanges()
        {
            logger.LogInformation("Saving changes to database.");
            return context.SaveChangesToDb();
        }
    }
}
