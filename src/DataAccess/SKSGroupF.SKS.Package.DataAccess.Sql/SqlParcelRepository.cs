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
            if (parcel == null)
                throw new ArgumentOutOfRangeException();

            parcel.Id = 1;
            return parcel.Id; 
        }

        public void Update(DALParcel parcel)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DALParcel> GetAll()
        {
            throw new NotImplementedException();
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

        public DALParcel GetByTrackingId(string tid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DALParcel> GetByWeight(int min, int max)
        {
            throw new NotImplementedException();
        }
    }
}
