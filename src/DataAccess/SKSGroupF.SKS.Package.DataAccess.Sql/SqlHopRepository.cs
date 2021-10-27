using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using System;
using System.Collections.Generic;

namespace SKSGroupF.SKS.Package.DataAccess.Sql
{
    public class SqlHopRepository : IHopRepository
    {
        public int Create(DALHop hop)
        {
            //Insert Warehouse and return db ID
            return 1;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DALHop> GetAll()
        {
            List<DALHop> WarehouseList = new List<DALHop>();
            DALHop w1 = new DALWarehouse();
            WarehouseList.Add(w1);
            DALHop w2 = new DALWarehouse();
            WarehouseList.Add(w2);
            DALHop w3 = new DALWarehouse();
            WarehouseList.Add(w3);

            return WarehouseList;
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

        public DALHop GetByCode(string code)
        {
            DALHop hop = new DALWarehouse();
            hop.Code = code;
            return hop;
        }

        public void Update(DALHop hop)
        {
            throw new NotImplementedException();
        }
    }
}
