using AutoMapper;
using SKSGroupF.SKS.Package.Services.DTOs.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Entities;

public class SenderApiProfile : Profile
{
    public SenderApiProfile()
    {
        CreateMap<Parcel, BLParcel>().ReverseMap();
        CreateMap<NewParcelInfo, BLParcel>().ReverseMap();
        CreateMap<TrackingInformation, BLParcel>().ReverseMap();
    }
}
