using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Interfaces
{
    public interface IWarehouseLogic
    {
        public string ExportWarehouses();
        public string GetWarehouse(string code);
        public void ImportWarehouses(/*Warehouse warehouse*/);
    }
}
