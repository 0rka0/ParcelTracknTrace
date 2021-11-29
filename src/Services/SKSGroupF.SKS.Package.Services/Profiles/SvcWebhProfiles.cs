using AutoMapper;
using SKSGroupF.SKS.Package.Services.DTOs.Models;

public class SvcWebhProfiles : Profile
{
    public SvcWebhProfiles()
    {
        CreateMap<WebhookResponse, SKSGroupF.SKS.Package.Webhooks.Entities.WebhookResponse>().ReverseMap();
        CreateMap<WebhookResponses, SKSGroupF.SKS.Package.Webhooks.Entities.WebhookResponses>().ReverseMap();
    }
}