namespace TradeGenius.WebApi.Infrastructure.MQTTClient.SettingsModel;

public class MqttSettings
{
    public BrokerHostSettings BrokerHostSettings { get; set; }
    public ClientSettings ClientSettings { get; set; }
    public List<string> Topics { get; set; } = new();
}