using Newtonsoft.Json;
using TradeGenius.WebApi.Application.Common.Brokering;
using TradeGenius.WebApi.Application.Common.MQTT;
using TradeGenius.WebApi.Application.Common.RecurringJob;
using TradeGenius.WebApi.Application.Crypto.Enums;

namespace TradeGenius.WebApi.Application.Crypto.Coins.RecurringJobs;

public class CoinCapJob : IJobRecurringService
{
    public string Id => nameof(CoinCapJob);

    public string Time => RecurringTime.MinuteInterval(3);

    public TimeZoneInfo TimeZone => TimeZoneInfo.Utc;

    public string Qoeue => "default";

    private readonly Func<BrokerTypes, IBrokerService> _brokerService;

    private readonly IMqttClientService _mqttClientService;

    public CoinCapJob(Func<BrokerTypes, IBrokerService> brokerService, MqttClientServiceProvider provider) =>
        (_brokerService, _mqttClientService) = (brokerService, provider.MqttClientService);

    public async Task CheckOut()
    {
        var result = await this._brokerService(BrokerTypes.CoinCap).GetDataAsync(CancellationToken.None);

        if (result is not null && ((List<CoinCapDto>)result).Any())
            await _mqttClientService.PublishAsync(MqttTopics.CoinCap, JsonConvert.SerializeObject(result));
    }
}
