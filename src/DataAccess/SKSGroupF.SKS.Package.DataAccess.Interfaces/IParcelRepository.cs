using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using System.Collections.Generic;

namespace SKSGroupF.SKS.Package.DataAccess.Interfaces
{
    public interface IParcelRepository
    {
        int Create(DALParcel parcel);
        void Update(DALParcel parcel);
        void Delete(int id);

        IEnumerable<DALParcel> GetAll();
        IEnumerable<DALParcel> GetByWeight(int min, int max);
        IEnumerable<DALParcel> GetBySender(DALReceipient sender);
        IEnumerable<DALParcel> GetByReceipient(DALReceipient receipient);
        IEnumerable<DALParcel> GetByState(DALParcel.StateEnum state);

        DALParcel GetByTrackingId(string tid);
    }
}
