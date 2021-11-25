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
        List<DALHopArrival> GetHopArrivalsByParcel(DALParcel parcel, bool visited);

        IEnumerable<DALParcel> GetAll();
       // IEnumerable<DALParcel> GetByWeight(float min, float max);
       // IEnumerable<DALParcel> GetBySender(DALReceipient sender);
       // IEnumerable<DALParcel> GetByReceipient(DALReceipient receipient);
       // IEnumerable<DALParcel> GetByState(DALParcel.StateEnum state);

        void UpdateHopState(string trackingId, string code, DALHop hop);
        void UpdateDelivered(DALParcel parcel);

        int SaveChanges();
    }
}
