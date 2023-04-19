using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TradeGenius.WebApi.Application.Common.Brokering;
using TradeGenius.WebApi.Infrastructure.CoinCap.Models;

namespace TradeGenius.WebApi.Infrastructure.CoinCap;
public class CoinCapService : IBrokerService
{
    private readonly CoinCapSettings _settings;
    private readonly ILogger<CoinCapService> _logger;

    public CoinCapService(IOptions<CoinCapSettings> settings, ILogger<CoinCapService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task GetDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var client = new HttpClient();
            string response = await client.GetStringAsync(_settings.RESTfullAPI + "assets");

            List<Coin> coins = JObject.Parse(response)["data"].Select(d => d.ToObject<Coin>()).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
