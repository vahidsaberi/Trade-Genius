namespace TradeGenius.WebApi.Shared.Events;
public interface IEventMapper
{
    IIntegrationEvent MapToIntegrationEvent(IDomainEvent @event);
    IInternalCommand MapToInternalCommand(IDomainEvent @event);
}
