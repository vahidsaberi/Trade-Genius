using TradeGenius.WebApi.Application.Crypto.Coins;

namespace TradeGenius.WebApi.Application.Common.Brokering;

public interface IBrokerService : ITransientService
{
    Task<List<CoinCapDto>> GetDataAsync(CancellationToken cancellationToken = default);
}
