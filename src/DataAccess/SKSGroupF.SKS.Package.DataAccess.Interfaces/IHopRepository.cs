using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.DataAccess.Interfaces
{
    public interface IHopRepository
    {
        int Create(DALHop hop);
        void Update(DALHop hop);
        void Delete(int id);

        IEnumerable<DALHop> GetAll();
        /*IEnumerable<DALHop> GetAllWarehouses();
        IEnumerable<DALHop> GetAllTrucks();
        IEnumerable<DALHop> GetAllTransferWarehouse();*/

        DALHop GetByCode(string code);

        /*IEnumerable<DALHop> GetByLevel(int level);
        IEnumerable<DALHop> GetByLogisticsPartner(string partner);
        IEnumerable<DALHop> GetByNumberPlate(string number);*/
    }
}
