using global::MassTransit;

namespace TradeGenius.WebApi.Shared.Events;

public interface IEvent
{
    Guid EventId => NewId.NextGuid();
    public DateTime OccurredOn => DateTime.Now;
    public string EventType => GetType().AssemblyQualifiedName;

}