﻿namespace TradeGenius.WebApi.Infrastructure.BackgroundJobs.RecurringJobs;

public interface IRecurringJobInitialization
{
    void InitializeRecurringJobs();
    Task InitializeJobsForTenantAsync(CancellationToken cancellationToken);
}
