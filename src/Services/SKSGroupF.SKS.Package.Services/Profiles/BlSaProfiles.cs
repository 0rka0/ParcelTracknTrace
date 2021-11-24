using AutoMapper;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.ServiceAgents;
using SKSGroupF.SKS.Package.ServiceAgents.Entities;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class BlSaProfiles : Profile
{
    public BlSaProfiles()
    {
        CreateMap<BLReceipient, SAReceipient>().ReverseMap();

        CreateMap<BLGeoCoordinate, SAGeoCoordinate>().ReverseMap();
    }
}

