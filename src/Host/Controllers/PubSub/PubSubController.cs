using TradeGenius.WebApi.Application.PubSub;

namespace TradeGenius.WebApi.Host.Controllers.PubSub;

public class PubSubController : VersionedApiController
{
    [HttpPost]
    [MustHavePermission(TradeGeniusAction.Create, TradeGeniusResource.PubSub)]
    [OpenApiOperation("Send message.", "")]
    public Task<bool> CreateAsync(CreatePubSubRequest request)
    {
        return Mediator.Send(request);
    }
}
