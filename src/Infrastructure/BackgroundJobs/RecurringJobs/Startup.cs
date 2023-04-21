using Microsoft.Extensions.DependencyInjection;
using TradeGenius.WebApi.Infrastructure.Common;

namespace TradeGenius.WebApi.Infrastructure.BackgroundJobs.RecurringJobs;
internal static class Startup
{
    internal static IServiceCollection AddRecurringBackgroundJobs(this IServiceCollection services)
    {
        services.AddServices(typeof(IRecurringJobInitialization), ServiceLifetime.Transient);

        return services;
    }
}
