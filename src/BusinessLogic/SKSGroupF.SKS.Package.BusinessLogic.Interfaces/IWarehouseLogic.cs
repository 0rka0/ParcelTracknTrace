using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Interfaces
{
    public interface IWarehouseLogic
    {
        public IEnumerable<BLWarehouse> ExportWarehouses();
        public BLWarehouse GetWarehouse(string code);
        public void ImportWarehouses(BLWarehouse warehouse);
    }
}
