using AutoMapper;
using SKSGroupF.SKS.Package.Services.DTOs.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;

public class SvcBlProfiles : Profile
{
    public SvcBlProfiles()
    {
        //Parcel --> BLParcel
        CreateMap<Parcel, BLParcel>().ReverseMap();
        CreateMap<NewParcelInfo, BLParcel>().ReverseMap();
        CreateMap<TrackingInformation, BLParcel>().ReverseMap();

        CreateMap<Receipient, BLReceipient>().ReverseMap();

        CreateMap<Hop, BLHop>().ReverseMap();

        CreateMap<HopArrival, BLHopArrival>().ReverseMap();

        CreateMap<Error, BLError>().ReverseMap();

        CreateMap<GeoCoordinate, BLGeoCoordinate>().ReverseMap();

        CreateMap<Transferwarehouse, BLTransferWarehouse>().ReverseMap();

        CreateMap<Truck, BLTruck>().ReverseMap();

        CreateMap<Warehouse, BLWarehouse>().ReverseMap();

        CreateMap<WarehouseNextHops, BLWarehouseNextHops>().ReverseMap();
    }
}
