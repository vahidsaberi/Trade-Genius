using TradeGenius.WebApi.Shared.Events;

namespace TradeGenius.WebApi.Domain.Common.BuildingBlocks;
public interface IAggregate<T> : IAggregate, IEntity<T>
{
}

public interface IAggregate : IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IEvent[] ClearDomainEvents();
}
