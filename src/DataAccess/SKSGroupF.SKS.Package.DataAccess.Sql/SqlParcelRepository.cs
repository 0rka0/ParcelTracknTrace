using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace SKSGroupF.SKS.Package.DataAccess.Sql
{
    public class SqlParcelRepository : IParcelRepository
    {
        public int Create(DALParcel parcel)
        {
            //Insert Parcel and return db ID
            return 1; 
        }

        public void Update(DALParcel parcel)
        {
            //Update trackingID of parcel to match received parcel
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DALParcel> GetAll()
        {
            throw new NotImplementedException();
        }

        public DALParcel GetByTrackingId(string tid)
        {
            //Return parcel by trackingID
            DALParcel parcel = new DALParcel();
            parcel.TrackingId = tid;
            return parcel;
        }

        public IEnumerable<DALParcel> GetByReceipient(DALReceipient receipient)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DALParcel> GetBySender(DALReceipient sender)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DALParcel> GetByState(DALParcel.StateEnum state)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DALParcel> GetByWeight(int min, int max)
        {
            throw new NotImplementedException();
        }

        public void UpdateHopState(DALParcel parcel, string code)
        {
            //Selects FutureHop by Code and marks it as VisitedHop
        }

        public void UpdateDelivered()
        {
            //Marks parcel as delievered
        }
    }
}
