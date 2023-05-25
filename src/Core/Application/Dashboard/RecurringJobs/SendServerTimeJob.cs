using TradeGenius.WebApi.Application.Common.MQTT;
using TradeGenius.WebApi.Application.Common.RecurringJob;

namespace TradeGenius.WebApi.Application.Dashboard.RecurringJobs;
public class SendServerTimeJob : IJobRecurringService
{
    public string Id => nameof(SendServerTimeJob);

    public string Time => RecurringTime.Minutely();

    public TimeZoneInfo TimeZone => TimeZoneInfo.Utc;

    public string Qoeue => "default";

    private readonly IMqttClientService _mqttClientService;

    public SendServerTimeJob(MqttClientServiceProvider provider) =>
        (_mqttClientService) = (provider.MqttClientService);

    public async Task CheckOut()
    {
        await _mqttClientService.PublishAsync(MqttTopics.ServerTime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}
