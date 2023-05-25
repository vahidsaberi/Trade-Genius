using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TradeGenius.WebApi.Application.Common.Brokering;
using TradeGenius.WebApi.Application.Crypto.Coins;

namespace TradeGenius.WebApi.Infrastructure.CryptoCurrency.CryptoCompare;
public class CryptoCompareService : IBrokerService
{
    private readonly CryptoCompareSettings _settings;
    private readonly ILogger<CryptoCompareService> _logger;

    public CryptoCompareService(IOptions<CryptoCompareSettings> settings, ILogger<CryptoCompareService> logger) =>
        (_settings, _logger) = (settings.Value, logger);

    public async Task<dynamic> GetDataAsync(CancellationToken cancellationToken = default)
    {
        var coins = new List<CryptoCompareDto>();

        try
        {
            var client = new HttpClient();

            // Replace {FROM_SYMBOLS} with the cryptocurrency symbols you want to retrieve data for, separated by commas (e.g., BTC,ETH,LTC)
            string[] fromSymbols = { "BTC", "ETH", "LTC" };
            string fromSymbolsString = string.Join(",", fromSymbols);

            // Replace {TO_SYMBOLS} with the currency symbols you want to convert to, separated by commas (e.g., USD,EUR)
            string[] toSymbols = { "USD" };
            string toSymbolsString = string.Join(",", toSymbols);

            string url = _settings.RESTfullAPI.Replace("{FROM_SYMBOLS}", fromSymbolsString).Replace("{TO_SYMBOLS}", toSymbolsString);

            string response = await client.GetStringAsync(url);

            var data = JObject.Parse(response);

            if (data is null) return coins;

            var result = data.SelectTokens("$.RAW.*.USD").ToList();

            foreach (var item in result)
            {
                var json = item.ToString();

                if (string.IsNullOrWhiteSpace(json)) continue;

                var coin = JsonConvert.DeserializeObject<CryptoCompareDto>(json);
                coins.Add(coin);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return coins;
    }
}
