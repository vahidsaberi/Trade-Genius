using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TradeGenius.WebApi.Application.Common.Brokering;
using TradeGenius.WebApi.Application.Crypto.Coins;

namespace TradeGenius.WebApi.Infrastructure.CryptoCurrency.YahooFinancial;
public class YahooFinancialService : IBrokerService
{
    private readonly YahooFinancialSettings _settings;
    private readonly ILogger<YahooFinancialService> _logger;

    public YahooFinancialService(IOptions<YahooFinancialSettings> settings, ILogger<YahooFinancialService> logger) =>
        (_settings, _logger) = (settings.Value, logger);

    public async Task<dynamic> GetDataAsync(CancellationToken cancellationToken = default)
    {
        var coins = new List<YahooFinancialDto>();

        try
        {
            var client = new HttpClient();
            string response = await client.GetStringAsync(_settings.RESTfullAPI);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return coins;
    }
}
