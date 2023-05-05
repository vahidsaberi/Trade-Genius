using TradeGenius.WebApi.Application.Common.MQTT;

namespace TradeGenius.WebApi.Infrastructure.MQTTClient;

public class MqttService
{
    private readonly IMqttClientService mqttClientService;

    public MqttService(MqttClientServiceProvider provider)
    {
        mqttClientService = provider.MqttClientService;
    }
}
