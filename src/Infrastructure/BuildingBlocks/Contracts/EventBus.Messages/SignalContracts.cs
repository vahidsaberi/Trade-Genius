using TradeGenius.WebApi.Shared.Events;

namespace TradeGenius.WebApi.Infrastructure.BuildingBlocks.Contracts.EventBus.Messages;

public record SignalCreated(Guid Id) : IIntegrationEvent;