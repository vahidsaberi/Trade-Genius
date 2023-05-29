using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeGenius.WebApi.Application.Common.Brokering;
using TradeGenius.WebApi.Application.Common.MQTT;
using TradeGenius.WebApi.Application.Common.RecurringJob;
using TradeGenius.WebApi.Application.Crypto.Enums;

namespace TradeGenius.WebApi.Application.Crypto.Coins.RecurringJobs;
public class CryptoCompareJob : IJobRecurringService
{
    public string Id => nameof(CryptoCompareJob);

    public string Time => RecurringTime.MinuteInterval(3);

    public TimeZoneInfo TimeZone => TimeZoneInfo.Utc;

    public string Qoeue => "default";

    private readonly Func<BrokerTypes, IBrokerService> _brokerService;

    private readonly IMqttClientService _mqttClientService;

    public CryptoCompareJob(Func<BrokerTypes, IBrokerService> brokerService, MqttClientServiceProvider provider) =>
        (_brokerService, _mqttClientService) = (brokerService, provider.MqttClientService);

    public async Task CheckOut()
    {
        var result = await this._brokerService(BrokerTypes.CryptoCompare).GetDataAsync(CancellationToken.None);

        //if (result is not null && ((List<co>)result).Any())
        //    await _mqttClientService.PublishAsync(MqttTopics.CoinCap, JsonConvert.SerializeObject(result));
    }
}
