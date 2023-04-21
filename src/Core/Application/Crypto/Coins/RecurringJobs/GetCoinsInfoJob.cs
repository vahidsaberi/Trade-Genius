using TradeGenius.WebApi.Application.Common.Brokering;
using TradeGenius.WebApi.Application.Common.RecurringJob;

namespace TradeGenius.WebApi.Application.Crypto.Coins.RecurringJobs;

public class GetCoinsInfoJob : IJobRecurringService
{
    public string Id => nameof(GetCoinsInfoJob);

    public string Time => RecurringTime.MinuteInterval(3);

    public TimeZoneInfo TimeZone => TimeZoneInfo.Utc;

    public string Qoeue => "default";


    private readonly IBrokerService _brokerService;

    public GetCoinsInfoJob(IBrokerService brokerService)
    {
        _brokerService = brokerService;
    }

    public async Task CheckOut()
    {
        var result = await _brokerService.GetDataAsync(CancellationToken.None);
    }
}
