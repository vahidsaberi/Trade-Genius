using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TradeGenius.WebApi.Infrastructure.CoinCap;
internal static class Startup
{
    internal static IServiceCollection AddCoinCap(this IServiceCollection services, IConfiguration config) =>
        services.Configure<CoinCapSettings>(config.GetSection(nameof(CoinCapSettings)));
}
