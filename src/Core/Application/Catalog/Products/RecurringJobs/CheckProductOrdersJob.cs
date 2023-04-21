using TradeGenius.WebApi.Application.Common.RecurringJob;

namespace TradeGenius.WebApi.Application.Catalog.Products.RecurringJobs;

public class CheckProductOrdersJob : IJobRecurringService
{
    public string Id => nameof(CheckProductOrdersJob);

    public string Time => RecurringTime.Daily();

    public TimeZoneInfo TimeZone => TimeZoneInfo.Utc;

    public string Qoeue => "default";

    public async Task CheckOut()
    {
        // This is just example of task
    }
}
