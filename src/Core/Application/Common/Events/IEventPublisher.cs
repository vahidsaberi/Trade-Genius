using TradeGenius.WebApi.Shared.Events;

namespace TradeGenius.WebApi.Application.Common.Events;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}