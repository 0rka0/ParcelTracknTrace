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
                throw new DALSqlContextException(nameof(SqlHopRepository), nameof(Create), errorMsg, ex);
            }
        }

        public void Delete(int id)
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

        public void Clear()
        {
            logger.LogInformation("Clearing all hops from database.");

            try
            {
                foreach (var geo in context.DbGeoCoordinate)
                    context.DbGeoCoordinate.Remove(geo);

                foreach (var wnh in context.DbWarehouseNextHops)
                    context.DbWarehouseNextHops.Remove(wnh);

                foreach (var hop in context.DbHop)
                    context.DbHop.Remove(hop);
                SaveChanges();
            }
            catch (Exception ex)
            {
                string errorMsg = "An error occurred when removing hop from database.";
                logger.LogError(errorMsg);
                throw new DALSqlContextException(nameof(SqlHopRepository), nameof(Clear), errorMsg, ex);
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
                logger.LogError(errorMsg, ex);
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
                logger.LogError(errorMsg, ex);
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
                logger.LogError(errorMsg, ex);
                throw new DALDataNotFoundException(nameof(SqlHopRepository), nameof(GetAll), errorMsg, ex);
            }
        }

        int SaveChanges()
        {
            logger.LogInformation("Saving changes to database.");
            return context.SaveChangesToDb();
        }
    }
}
