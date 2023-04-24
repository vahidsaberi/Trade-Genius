using Microsoft.Extensions.Hosting;

namespace TradeGenius.WebApi.Application.Common.Interfaces.MQTT;

public interface IMqttClientService : IHostedService, ITransientService
{
    Task<bool> SubscribeAsync(List<string> topics);
    Task<bool> UnsubscribeAsync(List<string> topics);
    Task<bool> PublishAsync(string topic, string message);
}
