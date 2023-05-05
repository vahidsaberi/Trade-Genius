using TradeGenius.WebApi.Application.PubSub;

namespace TradeGenius.WebApi.Host.Controllers.PubSub;

public class PubSubController : VersionedApiController
{
    [HttpPost("subscribe")]
    [MustHavePermission(TradeGeniusAction.Generate, TradeGeniusResource.PubSub)]
    [OpenApiOperation("Subscribe topics.", "")]
    public Task<bool> SubscribeAsync(CreateSubscribeRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("unsubscribe")]
    [MustHavePermission(TradeGeniusAction.Clean, TradeGeniusResource.PubSub)]
    [OpenApiOperation("Unsubscribe topics.", "")]
    public Task<bool> UnsubscribeAsync(CreateUnsubscribeRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("publish")]
    [MustHavePermission(TradeGeniusAction.Create, TradeGeniusResource.PubSub)]
    [OpenApiOperation("Send message.", "")]
    public Task<bool> PublishAsync(CreatePublishRequest request)
    {
        return Mediator.Send(request);
    }
}
