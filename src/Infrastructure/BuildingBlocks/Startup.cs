using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace TradeGenius.WebApi.Infrastructure.BuildingBlocks;
internal static class Startup
{
    private static readonly ILogger _logger = Log.ForContext(typeof(Startup));

    internal static IServiceCollection AddCustomCap<TDbContext>(this IServiceCollection services, IConfiguration config) where TDbContext : DbContext
    {
        services.Configure<RabbitMQOptions>(config.GetSection(nameof(RabbitMQOptions)));
        var options = config.GetSection(nameof(RabbitMQOptions)).Get<RabbitMQOptions>();
        if (options is null) throw new Exception("MQTT client is not configured.");

        services.AddCap(x =>
        {
            x.UseEntityFramework<TDbContext>();
            x.UseRabbitMQ(o =>
            {
                o.HostName = options.HostName;
                o.UserName = options.UserName;
                o.Password = options.Password;
            });
            x.UseDashboard();
            x.FailedRetryCount = 5;
            x.FailedThresholdCallback = failed =>
            {
                var logger = failed.ServiceProvider.GetService<ILogger>();
                _logger.Error($@"A message of type {failed.MessageType} failed after executing {x.FailedRetryCount} several times,
                        requiring manual troubleshooting. Message name: {failed.Message.GetName()}");
            };
            x.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        });

        services.AddScoped<ICapSubscribe>();

        return services;
    }
}
