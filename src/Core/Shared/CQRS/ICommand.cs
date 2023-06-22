using MediatR;

namespace TradeGenius.WebApi.Shared.CQRS;
public interface ICommand : ICommand<Unit>
{
}

public interface ICommand<out T> : IRequest<T>
    where T : notnull
{
}
