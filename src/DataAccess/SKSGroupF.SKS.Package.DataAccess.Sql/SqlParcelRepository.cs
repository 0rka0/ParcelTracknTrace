﻿using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SKSGroupF.SKS.Package.DataAccess.Sql
{
    public class SqlParcelRepository : IParcelRepository
    {
        private ISqlDbContext context;

        public SqlParcelRepository()
        {
            context = new SqlDbContext();
        }

        public SqlParcelRepository(ISqlDbContext context)
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
            try
            {
                return context.DbParcel.ToList();
            }
            catch { return null; }
        }

        public DALParcel GetByTrackingId(string tid)
        {
            try
            {
                return context.DbParcel.Single(p => p.TrackingId == tid);
            }
            catch { return null; }
        }

        public IEnumerable<DALParcel> GetByReceipient(DALReceipient receipient)
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