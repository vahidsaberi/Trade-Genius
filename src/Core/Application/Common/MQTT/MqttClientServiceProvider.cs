namespace TradeGenius.WebApi.Application.Common.MQTT;

public class MqttClientServiceProvider
{
    public readonly IMqttClientService MqttClientService;

    public MqttClientServiceProvider(IMqttClientService mqttClientService)
    {
        MqttClientService = mqttClientService;
    }
}
