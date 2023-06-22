using TradeGenius.WebApi.Shared.CQRS;
using TradeGenius.WebApi.Shared.Events;

namespace TradeGenius.WebApi.Domain.Common.Events;
public record InternalCommand : IInternalCommand, ICommand;