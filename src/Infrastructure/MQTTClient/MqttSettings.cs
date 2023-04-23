namespace TradeGenius.WebApi.Infrastructure.MQTTClient;

public class MqttSettings
{
    public BrokerHostSettings BrokerHostSettings { get; set; }
    public ClientSettings ClientSettings { get; set; }
}