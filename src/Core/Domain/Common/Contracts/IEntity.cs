using TradeGenius.WebApi.Domain.Common.BuildingBlocks;

namespace TradeGenius.WebApi.Domain.Common.Contracts;

public interface IEntity: IVersion
{
    List<DomainEvent> DomainEvents { get; }
}

public interface IEntity<TId> : IEntity
{
    TId Id { get; }
}