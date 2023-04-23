using TradeGenius.WebApi.Application.Common.Interfaces.MQTT;

namespace TradeGenius.WebApi.Infrastructure.MQTTClient;

public class MqttService
{
    private readonly IMqttClientService mqttClientService;

    public MqttService(MqttClientServiceProvider provider)
    {
        mqttClientService = provider.MqttClientService;
    }
}
