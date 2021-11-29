using AutoMapper;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.Webhooks.Entities;

public class WebhDalProfiles : Profile
{
    public WebhDalProfiles()
    {
        CreateMap<WebhookResponse, DALWebhookResponse>().ReverseMap();
        CreateMap<WebhookResponses, DALWebhookResponses>().ReverseMap();
    }
}
