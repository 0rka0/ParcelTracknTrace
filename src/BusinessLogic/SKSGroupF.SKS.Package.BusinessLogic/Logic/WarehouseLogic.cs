using FluentValidation;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Validators;
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
            var blWarehouse = new BLWarehouse();
            blWarehouse.Code = code;

            IValidator<string> validator = new StringValidator(false);

            var result = validator.Validate(code);

            if (result.IsValid)
                return blWarehouse;

            throw new ArgumentOutOfRangeException();
        }

        public void ImportWarehouses(BLWarehouse warehouse)
        {
            IValidator<BLWarehouse> validator = new WarehouseValidator();
            
            var result = validator.Validate(warehouse);

            if (!result.IsValid)
                throw new ArgumentOutOfRangeException();
        }
    }
}
