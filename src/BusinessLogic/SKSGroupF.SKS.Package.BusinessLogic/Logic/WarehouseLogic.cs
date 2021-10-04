using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Logic
{
    public class WarehouseLogic : IWarehouseLogic
    {
        public IEnumerable<BLWarehouse> ExportWarehouses()
        {
            try
            {
                List<BLWarehouse> WarehouseList = new List<BLWarehouse>();
                BLWarehouse w1 = new BLWarehouse();
                WarehouseList.Add(w1);
                BLWarehouse w2 = new BLWarehouse();
                WarehouseList.Add(w2);
                BLWarehouse w3 = new BLWarehouse();
                WarehouseList.Add(w3);

                return WarehouseList;
            }
            catch
            {
                throw new Exception();
            }
        }

        public BLWarehouse GetWarehouse(string code)
        {
            if(String.Compare(code, "AAAA\\dddd") == 0)
                return new BLWarehouse();

            throw new ArgumentOutOfRangeException();
        }

        public void ImportWarehouses(BLWarehouse warehouse)
        {
            throw new NotImplementedException();
        }
    }
}
