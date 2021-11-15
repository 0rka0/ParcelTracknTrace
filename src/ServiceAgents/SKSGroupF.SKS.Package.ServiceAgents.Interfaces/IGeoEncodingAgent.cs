using SKSGroupF.SKS.Package.ServiceAgents.Entities;

namespace SKSGroupF.SKS.Package.ServiceAgents.Interfaces
{
    public interface IGeoEncodingAgent
    {
        SAGeoCoordinate EncodeAddress(SAReceipient address);
    }
}
