using Microsoft.Extensions.Logging;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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
            logger.LogInformation("Inserting hop into database.");
            var hopEnt = context.DbHop.Add(hop);
            logger.LogInformation("Hop inserted with ID + " + hopEnt.Entity.Id + ".");
            SaveChanges();
            return hopEnt.Entity.Id;
        }

        public void Delete(int id)
        {
            var hop = context.DbHop.Find(id);
            context.DbHop.Remove(hop);
            SaveChanges();
        }

        public void Update(DALHop hop)
        {
            context.DbHop.Update(hop);
            SaveChanges();
        }

        public DALHop GetByCode(string code)
        {
            logger.LogInformation("Trying to select a single hop with code " + code + ".");
            return context.DbHop.Single(h => h.Code == code);
        }

        public IEnumerable<DALHop> GetAll()
        {
            logger.LogInformation("Trying to select all hops from database.");
            return context.DbHop.ToList();
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
