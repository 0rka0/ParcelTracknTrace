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

        public SqlHopRepository()
        {
            context = new SqlDbContext();
        }

        public SqlHopRepository(ISqlDbContext context)
        {
            this.context = context;
        }

        public int Create(DALHop hop)
        {
            //Insert Warehouse and return db ID        
            var hopEnt = context.DbHop.Add(hop);
            context.SaveChangesToDb();
            return hopEnt.Entity.Id;
        }

        public void Delete(int id)
        {
            var hop = context.DbHop.Find(id);
            context.DbHop.Remove(hop);
            context.SaveChangesToDb();
        }

        public void Update(DALHop hop)
        {
            context.DbHop.Update(hop);
            context.SaveChangesToDb();
        }

        public DALHop GetByCode(string code)
        {
            return context.DbHop.Single(h => h.Code == code);
        }

        public IEnumerable<DALHop> GetAll()
        {
            return context.DbHop.ToList();
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
