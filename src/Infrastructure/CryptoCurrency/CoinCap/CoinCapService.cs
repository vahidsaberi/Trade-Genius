using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using TradeGenius.WebApi.Application.Common.Brokering;
using TradeGenius.WebApi.Application.Crypto.Coins;

namespace TradeGenius.WebApi.Infrastructure.CryptoCurrency.CoinCap;

public class CoinCapService : IBrokerService
{
    private readonly CoinCapSettings _settings;
    private readonly ILogger<CoinCapService> _logger;

    public CoinCapService(IOptions<CoinCapSettings> settings, ILogger<CoinCapService> logger) =>
        (_settings, _logger) = (settings.Value, logger);

    public async Task<dynamic> GetDataAsync(CancellationToken cancellationToken = default)
    {
        var coins = new List<CoinCapDto>();

        try
        {
            var client = new HttpClient();
            string response = await client.GetStringAsync(_settings.RESTfullAPI + "assets");

            coins = JObject.Parse(response)["data"].Select(d => d.ToObject<CoinCapDto>()).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return coins;
    }
}
