using AutoMapper;
using SKSGroupF.SKS.Package.Services.DTOs.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;

public class BlDalProfiles : Profile
{
    public BlDalProfiles()
    {
        //Parcel --> BLParcel
        CreateMap<BLParcel, DALParcel>().ReverseMap();

        CreateMap<BLReceipient, DALReceipient>().ReverseMap();

        CreateMap<BLHop, DALHop>().ReverseMap();

        CreateMap<BLHopArrival, DALHopArrival>().ReverseMap();

        CreateMap<BLError, DALError>().ReverseMap();

        CreateMap<BLGeoCoordinate, DALGeoCoordinate>().ReverseMap();

        CreateMap<BLTransferWarehouse, DALTransferWarehouse>().ReverseMap();

        CreateMap<BLTruck, DALTruck>().ReverseMap();

        CreateMap<BLWarehouse, DALWarehouse>().ReverseMap();

        CreateMap<BLWarehouseNextHops, DALWarehouseNextHops>().ReverseMap();
    }
}
