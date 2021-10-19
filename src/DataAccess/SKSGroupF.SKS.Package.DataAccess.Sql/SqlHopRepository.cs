using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using System;
using System.Collections.Generic;

namespace SKSGroupF.SKS.Package.DataAccess.Sql
{
    class SqlHopRepository : IHopRepository
    {
        public int Create(DALHop hop)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DALHop> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DALHop> GetAllTransferWarehouse()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DALHop> GetAllTrucks()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DALHop> GetAllWarehouses()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DALHop> GetByLevel(int level)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DALHop> GetByLogisticsPartner(string partner)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DALHop> GetByNumberPlate(string number)
        {
            throw new NotImplementedException();
        }

        public DALHop SelectByCode(string code)
        {
            throw new NotImplementedException();
        }

        public void Update(DALHop hop)
        {
            throw new NotImplementedException();
        }
    }
}
