using MediatR;

namespace TradeGenius.WebApi.Shared.CQRS;
public interface IQuery<out T> : IRequest<T>
    where T : notnull
{
}
