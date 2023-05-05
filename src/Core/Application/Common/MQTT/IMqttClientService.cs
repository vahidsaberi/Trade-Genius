using Microsoft.Extensions.Hosting;

namespace TradeGenius.WebApi.Application.Common.MQTT;

public interface IMqttClientService : IHostedService, ITransientService
{
    Task<bool> SubscribeAsync(List<string> topics);
    Task<bool> UnsubscribeAsync(List<string> topics);
    Task<bool> PublishAsync(MqttTopics topic, string message);
}
