using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Linq;

namespace SKSGroupF.SKS.Package.DataAccess.Sql
{
    public class SqlParcelRepository : IParcelRepository
    {
        private ISqlDbContext context;

        SqlParcelRepository()
        {
            context = new SqlDbContext();
        }

        SqlParcelRepository(ISqlDbContext context)
        {
            this.context = context;
        }

        public int Create(DALParcel parcel)
        {
            //Insert Parcel and return db ID
            var ent = context.DbParcel.Add(parcel);
            context.SaveChangesToDb();
            return ent.Entity.Id;
        }

        public void Update(DALParcel parcel)
        {
            context.DbParcel.Update(parcel);
            context.SaveChangesToDb();
        }

        public void Delete(int id)
        {
            var parcel = context.DbParcel.Find(id);
            context.DbParcel.Remove(parcel);
            context.SaveChangesToDb();
        }

        public IEnumerable<DALParcel> GetAll()
        {
            return context.DbParcel.ToList();
        }

        public DALParcel GetByTrackingId(string tid)
        {
            return context.DbParcel.Single(p => p.TrackingId == tid);
        }

        public IEnumerable<DALParcel> GetByReceipient(DALReceipient receipient)
        {
            return context.DbParcel.Where(p => p.Receipient == receipient).ToList();
        }

        public IEnumerable<DALParcel> GetBySender(DALReceipient sender)
        {
            return context.DbParcel.Where(p => p.Sender == sender).ToList();
        }

        public IEnumerable<DALParcel> GetByState(DALParcel.StateEnum state)
        {
            return context.DbParcel.Where(p => p.State == state).ToList();
        }

        public IEnumerable<DALParcel> GetByWeight(int min, int max)
        {
            return context.DbParcel.Where(p => p.Weight >= min && p.Weight <= max).ToList();
        }

        public void UpdateHopState(DALParcel parcel, string code)
        {
            //?
            var hop = context.DbHopArrival.Single(p => p.Code == code);
            parcel.FutureHops.Remove(hop);
            parcel.VisitedHops.Add(hop);
            context.DbParcel.Update(parcel);
            
            //Selects FutureHop by Code and marks it as VisitedHop
        }

        public void UpdateDelivered(DALParcel parcel)
        {
            parcel.Delievered = true;
            context.DbParcel.Update(parcel);

            //Sets parcel as delievered ??
        }
    }
}
