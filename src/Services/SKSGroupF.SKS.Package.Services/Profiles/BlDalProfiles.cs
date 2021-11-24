using AutoMapper;
using SKSGroupF.SKS.Package.Services.DTOs.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using NetTopologySuite.Algorithm;
using NetTopologySuite.IO;
using NetTopologySuite.Features;

public class BlDalProfiles : Profile
{
    public BlDalProfiles()
    {
        //Parcel --> BLParcel
        CreateMap<BLParcel, DALParcel>().ReverseMap();

        CreateMap<BLReceipient, DALReceipient>().ReverseMap();

        CreateMap<BLHop, DALHop>()
            .Include<BLTruck, DALTruck>()
            .Include<BLTransferWarehouse, DALTransferWarehouse>()
            .Include<BLWarehouse, DALWarehouse>()
            .ReverseMap();

        CreateMap<BLHopArrival, DALHopArrival>().ReverseMap();

        CreateMap<BLError, DALError>().ReverseMap();

        CreateMap<BLGeoCoordinate, DALGeoCoordinate>().ReverseMap();

        CreateMap<BLTransferWarehouse, DALTransferWarehouse>().ReverseMap();

        CreateMap<BLTruck, DALTruck>().ReverseMap();

        CreateMap<BLWarehouse, DALWarehouse>().ReverseMap();

        CreateMap<BLWarehouseNextHops, DALWarehouseNextHops>().ReverseMap();

        /*CreateMap<BLTruck, DALTruck>().BeforeMap((s, d) =>
        {
            var reader = new GeoJsonReader();
            Feature g = reader.Read<Feature>(s.RegionGeoJson);
            if (!Orientation.IsCCW(g.Geometry.Coordinates))
                g.Geometry = g.Geometry.Reverse();
            d.Region = g.Geometry;
        });

        CreateMap<DALTruck, BLTruck>().BeforeMap((s, d) =>
        {
            var writer = new GeoJsonWriter();
            d.RegionGeoJson = writer.Write(new Feature()
            {
                Geometry = s.Region
            });
        });*/
    }
}
