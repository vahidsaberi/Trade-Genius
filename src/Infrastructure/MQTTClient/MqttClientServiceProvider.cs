using TradeGenius.WebApi.Application.Common.Interfaces.MQTT;

namespace TradeGenius.WebApi.Infrastructure.MQTTClient;

public class MqttClientServiceProvider
{
    public readonly IMqttClientService MqttClientService;

    public MqttClientServiceProvider(IMqttClientService mqttClientService)
    {
        MqttClientService = mqttClientService;
    }
}
