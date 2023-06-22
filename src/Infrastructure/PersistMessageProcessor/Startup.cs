using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradeGenius.WebApi.Infrastructure.MQTTClient.SettingsModel;

namespace TradeGenius.WebApi.Infrastructure.PersistMessageProcessor;
internal static class Startup
{
    private const string CorrelationId = "correlationId";

    public static IServiceCollection AddPersistMessageProcessor(this IServiceCollection services, IConfiguration config)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        return services;
    }

    internal static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
    {
        return app.Use(async (ctx, next) =>
        {
            if (!ctx.Request.Headers.TryGetValue(CorrelationId, out var correlationId))
                correlationId = Guid.NewGuid().ToString("N");

            ctx.Items[CorrelationId] = correlationId.ToString();
            await next();
        });
    }

    public static Guid GetCorrelationId(this HttpContext context)
    {
        context.Items.TryGetValue(CorrelationId, out var correlationId);
        return string.IsNullOrEmpty(correlationId?.ToString()) ? Guid.NewGuid() : new Guid(correlationId.ToString()!);
    }
}
