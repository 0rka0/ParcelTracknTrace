using Microsoft.Extensions.Logging;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using SKSGroupF.SKS.Package.DataAccess.Interfaces.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace SKSGroupF.SKS.Package.DataAccess.Sql
{
    public class SqlHopRepository : IHopRepository
    {
        private ISqlDbContext context;
        private readonly ILogger logger;

        public SqlHopRepository(ISqlDbContext context, ILogger<SqlHopRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public int Create(DALHop hop)
        {
            //Insert Warehouse and return db ID
            try
            {
                try
                {
                    GetByCode(hop.Code);
                }
                catch (DALDataNotFoundException) //Hop will only be created if GetByCode does not find a hop with this code in the database
                {
                    logger.LogInformation("Inserting hop into database.");
                    var hopEnt = context.DbHop.Add(hop);
                    logger.LogInformation("Hop inserted with ID + " + hopEnt.Entity.Id + ".");
                    SaveChanges();
                    return hopEnt.Entity.Id;
                }

                string errorMsg = "Hop with specified Code does already exsist in database.";
                logger.LogError(errorMsg);
                throw new DALDataDuplicateException(nameof(SqlHopRepository), nameof(Create), errorMsg);
            }
            catch (Exception ex)
            {
                string errorMsg = "There has been an error with the database";
                logger.LogError(errorMsg);
                throw new DALDataException(nameof(SqlHopRepository), nameof(Create), errorMsg, ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                logger.LogInformation("Removing existing hop from database.");
                var hop = context.DbHop.Find(id);
                if (hop == null)
                {
                    string errorMsg = "Hop with specified Id could not be found in database.";
                    logger.LogError(errorMsg);
                    throw new DALDataNotFoundException(nameof(SqlHopRepository), nameof(Delete), errorMsg);
                }
                context.DbHop.Remove(hop);
                SaveChanges();
            }
            catch (Exception ex)
            {
                string errorMsg = "There has been an error with the database";
                logger.LogError(errorMsg);
                throw new DALDataException(nameof(SqlHopRepository), nameof(Create), errorMsg, ex);
            }
        }

        public void Update(DALHop hop)
        {
            try
            {
                var hopTmp = context.DbHop.Find(hop.Id);
                if (hopTmp == null)
                {
                    string errorMsg = "Hop with specified Id could not be found in database.";
                    logger.LogError(errorMsg);
                    throw new DALDataNotFoundException(nameof(SqlHopRepository), nameof(Delete), errorMsg);
                }
                context.DbHop.Update(hop);
                SaveChanges();
            }
            catch (Exception ex)
            {
                string errorMsg = "There has been an error when accessing the database";
                logger.LogError(errorMsg);
                throw new DALDataException(nameof(SqlHopRepository), nameof(Create), errorMsg, ex);
            }
        }

        public DALHop GetByCode(string code)
        {
            try
            {
                logger.LogInformation("Trying to select a single hop with code " + code + ".");
                return context.DbHop.Single(h => h.Code == code);
            }
            catch (Exception ex)
            {
                string errorMsg = "Hop with specified code could not be found in Database.";
                logger.LogError(errorMsg);
                throw new DALDataNotFoundException(nameof(SqlHopRepository), nameof(GetByCode), errorMsg, ex);
            }
        }

        public IEnumerable<DALHop> GetAll()
        {
            try
            {
                logger.LogInformation("Trying to select all hops from database.");
                return context.DbHop.ToList();
            }
            catch (Exception ex)
            {
                string errorMsg = "No hops found in Database.";
                logger.LogError(errorMsg);
                throw new DALDataNotFoundException(nameof(SqlHopRepository), nameof(GetAll), errorMsg, ex);
            }
        }

        int SaveChanges()
        {
            logger.LogInformation("Saving changes to database.");
            return context.SaveChangesToDb();
        }

        /*public IEnumerable<DALHop> GetByLevel(int level)
        {
            return context.DbWarehouse.Where(wh => wh.Level == level).ToList();
        }

        public IEnumerable<DALHop> GetByLogisticsPartner(string partner)
        {
            return context.DbTransferWarehouse.Where(twh => twh.LogisticsPartner == partner).ToList();
        }

        public IEnumerable<DALHop> GetByNumberPlate(string number)
        {
            return context.DbTruck.Where(t => t.NumberPlate == number).ToList();
        }*/

        /*public IEnumerable<DALHop> GetAllTransferWarehouse()
        {
            return context.DbHop.Where(t => t.HopType == "TransferWarehouse").ToList();
        }

        public IEnumerable<DALHop> GetAllTrucks()
        {
            return context.DbTruck.ToList();
        }

        public IEnumerable<DALHop> GetAllWarehouses()
        {
            return context.DbWarehouse.ToList();
        }*/
    }
}
