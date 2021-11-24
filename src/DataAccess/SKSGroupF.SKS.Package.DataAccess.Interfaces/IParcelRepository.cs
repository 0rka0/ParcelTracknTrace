using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using System.Collections.Generic;

namespace SKSGroupF.SKS.Package.DataAccess.Interfaces
{
    public interface IParcelRepository
    {
        int Create(DALParcel parcel);
        void Update(DALParcel parcel);
        void Delete(int id);
        void Clear();

        DALParcel GetByTrackingId(string tid);
        DALHopArrival GetHopArrivalByCode(string code, DALParcel id);

        IEnumerable<DALParcel> GetAll();
       // IEnumerable<DALParcel> GetByWeight(float min, float max);
       // IEnumerable<DALParcel> GetBySender(DALReceipient sender);
       // IEnumerable<DALParcel> GetByReceipient(DALReceipient receipient);
       // IEnumerable<DALParcel> GetByState(DALParcel.StateEnum state);

        //void UpdateHopState(DALParcel parcel, string code);
        void UpdateDelivered(DALParcel parcel);
    }
}
