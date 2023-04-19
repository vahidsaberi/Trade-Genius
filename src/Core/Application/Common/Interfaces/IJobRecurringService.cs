namespace TradeGenius.WebApi.Application.Common.Interfaces;

public interface IJobRecurringService : ITransientService
{
    Task CheckOut();
}
