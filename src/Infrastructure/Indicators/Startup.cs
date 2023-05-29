using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradeGenius.WebApi.Application.Crypto.Enums;
using TradeGenius.WebApi.Application.Crypto.Interfaces;
using TradeGenius.WebApi.Application.Crypto.Models;

namespace TradeGenius.WebApi.Infrastructure.Indicators;

internal static class Startup
{
    internal static IServiceCollection AddIndicators(this IServiceCollection services, IConfiguration config)
    {
        services.AddTransient<MovingAverage>();
        services.AddTransient<MovingAverageConvergenceDivergence>();
        services.AddTransient<RelativeStrengthIndex>();

        services.AddSingleton<Func<IndicatorTypes, IndicatorConfig, IIndicator>>(provider => (key, config) =>
        {
            IIndicator indicator;

            switch (key)
            {
                case IndicatorTypes.MA:
                    indicator = provider.GetRequiredService<MovingAverage>();
                    indicator.SetConfig(config);
                    return indicator;
                case IndicatorTypes.MACD:
                    indicator = provider.GetRequiredService<MovingAverageConvergenceDivergence>();
                    indicator.SetConfig(config);
                    return indicator;
                case IndicatorTypes.RSI:
                    indicator = provider.GetRequiredService<RelativeStrengthIndex>();
                    indicator.SetConfig(config);
                    return indicator;
                default:
                    throw new NotImplementedException($"Service of type {key} is not implemented.");
            }
        });

        return services;
    }
}