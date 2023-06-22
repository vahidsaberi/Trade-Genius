using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TradeGenius.WebApi.Application.Common.Interfaces;
using TradeGenius.WebApi.Application.Common.RecurringJob;

namespace TradeGenius.WebApi.Infrastructure.PersistMessageProcessor.RecurringJobs;
public class PersistMessageJob : IJobRecurringService
{
    public string Id => nameof(PersistMessageJob);

    public string Time => RecurringTime.Minutely();

    public TimeZoneInfo TimeZone => TimeZoneInfo.Utc;

    public string Qoeue => "default";

    private readonly ILogger<PersistMessageJob> _logger;
    private readonly IServiceProvider _serviceProvider;

    private Task? _executingTask;

    public PersistMessageJob(
        ILogger<PersistMessageJob> logger,
        IServiceProvider serviceProvider) =>
        (_logger, _serviceProvider) = (logger, serviceProvider);

    public async Task CheckOut()
    {
        await ExecuteAsync(CancellationToken.None);
    }

    protected async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PersistMessage Background Service Start");

        _executingTask = ProcessAsync(stoppingToken);
    }

    private async Task ProcessAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IPersistMessageProcessor>();
                await service.ProcessAllAsync(stoppingToken);
            }

            var delay = TimeSpan.FromSeconds(30);

            await Task.Delay(delay, stoppingToken);
        }
    }
}
