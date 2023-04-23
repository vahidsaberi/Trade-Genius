using Microsoft.Extensions.Hosting;

namespace TradeGenius.WebApi.Application.Common.Interfaces.MQTT;

public interface IMqttClientService : IHostedService, ITransientService
{
    Task MessageSendAsync(string topic, string message);
}
