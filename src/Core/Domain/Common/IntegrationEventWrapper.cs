using TradeGenius.WebApi.Shared.Events;

namespace TradeGenius.WebApi.Domain.Common;
public record IntegrationEventWrapper<TDomainEventType>(TDomainEventType DomainEvent) : IIntegrationEvent
    where TDomainEventType : IDomainEvent;
