﻿using TradeGenius.WebApi.Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TradeGenius.WebApi.Infrastructure.Common;

namespace TradeGenius.WebApi.Infrastructure.BackgroundJobs.RecurringJobs.Initialization;

internal static class Startup
{
    internal static IServiceCollection AddRecurringBackgroundJobs(this IServiceCollection services)
    {
        services.AddServices(typeof(IRecurringJobInitialization), ServiceLifetime.Transient);

        return services;
    }
}
