using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet.Client;
using TradeGenius.WebApi.Infrastructure.MQTTClient.SettingsModel;

namespace TradeGenius.WebApi.Infrastructure.MQTTClient;

internal static class Startup
{
    public static IServiceCollection AddMqttClientHostedService(this IServiceCollection services, IConfiguration config)
    {
        //services.AddSingleton<MqttService>();

        services.Configure<MqttSettings>(config.GetSection(nameof(MqttSettings)));

        var settings = config.GetSection(nameof(MqttSettings)).Get<MqttSettings>();
        if (settings is null) throw new Exception("MQTT client is not configured.");

        services.AddMqttClientServiceWithConfig(aspOptionBuilder =>
        {
            aspOptionBuilder
            .WithCredentials(settings.ClientSettings.UserName, settings.ClientSettings.Password)
            .WithClientId(Guid.NewGuid().ToString())
            .WithTcpServer(settings.BrokerHostSettings.Host, settings.BrokerHostSettings.Port);
        });
        return services;
    }

    private static IServiceCollection AddMqttClientServiceWithConfig(this IServiceCollection services, Action<MqttClientOptionsBuilder> configure)
    {
        services.AddSingleton<MqttClientOptions>(serviceProvider =>
        {
            var optionBuilder = new MqttClientOptionsBuilder();
            configure(optionBuilder);
            return optionBuilder.Build();
        });
        services.AddSingleton<MqttClientService>();
        services.AddSingleton<IHostedService>(serviceProvider =>
        {
            return serviceProvider.GetService<MqttClientService>();
        });
        services.AddSingleton<MqttClientServiceProvider>(serviceProvider =>
        {
            var mqttClientService = serviceProvider.GetService<MqttClientService>();
            var mqttClientServiceProvider = new MqttClientServiceProvider(mqttClientService);
            return mqttClientServiceProvider;
        });
        return services;
    }
}