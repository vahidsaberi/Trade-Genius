namespace TradeGenius.WebApi.Application.Common.Interfaces;

public interface IJobRecurringService : ITransientService
{
    string Id { get; }
    string Time { get; }
    TimeZoneInfo TimeZone { get; }
    string Qoeue { get; }

    Task CheckOut();
}
