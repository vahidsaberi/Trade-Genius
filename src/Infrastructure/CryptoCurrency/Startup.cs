using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradeGenius.WebApi.Application.Common.Brokering;
using TradeGenius.WebApi.Application.Crypto.Enums;
using TradeGenius.WebApi.Infrastructure.CryptoCurrency.CoinCap;
using TradeGenius.WebApi.Infrastructure.CryptoCurrency.CryptoCompare;
using TradeGenius.WebApi.Infrastructure.CryptoCurrency.YahooFinancial;

namespace TradeGenius.WebApi.Infrastructure.CryptoCurrency;

internal static class Startup
{
    internal static IServiceCollection AddCryptoCurrencyService(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<YahooFinancialSettings>(config.GetSection(nameof(YahooFinancialSettings)));
        services.Configure<CoinCapSettings>(config.GetSection(nameof(CoinCapSettings)));
        services.Configure<CryptoCompareSettings>(config.GetSection(nameof(CryptoCompareSettings)));

        services.AddTransient<CoinCapService>();
        services.AddTransient<CryptoCompareService>();
        services.AddTransient<YahooFinancialService>();

        services.AddSingleton<Func<BrokerTypes, IBrokerService>>(provider => (key) =>
        {
            IBrokerService broker;

            switch (key)
            {
                case BrokerTypes.CoinCap:
                    broker = provider.GetRequiredService<CoinCapService>();
                    return broker;
                case BrokerTypes.CryptoCompare:
                    broker = provider.GetRequiredService<CryptoCompareService>();
                    return broker;
                case BrokerTypes.YahooFinancial:
                    broker = provider.GetRequiredService<YahooFinancialService>();
                    return broker;
                default:
                    throw new NotImplementedException($"Service of type {key} is not implemented.");
            }
        });

        return services;
    }
}
