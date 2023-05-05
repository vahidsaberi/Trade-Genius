using Newtonsoft.Json;
using TradeGenius.WebApi.Application.Common.Brokering;
using TradeGenius.WebApi.Application.Common.MQTT;
using TradeGenius.WebApi.Application.Common.RecurringJob;

namespace TradeGenius.WebApi.Application.Crypto.Coins.RecurringJobs;

public class GetCoinsInfoJob : IJobRecurringService
{
    public string Id => nameof(GetCoinsInfoJob);

    public string Time => RecurringTime.MinuteInterval(3);

    public TimeZoneInfo TimeZone => TimeZoneInfo.Utc;

    public string Qoeue => "default";

    private readonly IBrokerService _brokerService;
    private readonly IMqttClientService _mqttClientService;

    public GetCoinsInfoJob(IBrokerService brokerService, MqttClientServiceProvider provider) =>
        (_brokerService, _mqttClientService) = (brokerService, provider.MqttClientService);

    public async Task CheckOut()
    {
        var result = await _brokerService.GetDataAsync(CancellationToken.None);

        if (result is not null && result.Any())
            await _mqttClientService.PublishAsync(MqttTopics.CoinCap, JsonConvert.SerializeObject(result));

    }
}
