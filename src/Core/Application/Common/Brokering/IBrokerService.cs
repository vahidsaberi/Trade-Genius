namespace TradeGenius.WebApi.Application.Common.Brokering;

public interface IBrokerService
{
    Task<dynamic> GetDataAsync(CancellationToken cancellationToken = default);
}
