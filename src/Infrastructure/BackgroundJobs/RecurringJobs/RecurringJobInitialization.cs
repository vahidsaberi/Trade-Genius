using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TradeGenius.WebApi.Application.Common.Interfaces;
using TradeGenius.WebApi.Infrastructure.Multitenancy;

namespace TradeGenius.WebApi.Infrastructure.BackgroundJobs.RecurringJobs;

public class RecurringJobInitialization : IRecurringJobInitialization
{
    private readonly ILogger<RecurringJobInitialization> _logger;
    private readonly IJobService _jobService;
    private readonly TenantDbContext _tenantDbContext;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITenantInfo _tenantInfo;

    public RecurringJobInitialization(ILogger<RecurringJobInitialization> logger, IJobService jobService, TenantDbContext tenantDbContext, IServiceProvider serviceProvider, ITenantInfo tenantInfo)
    {
        _logger = logger;
        _jobService = jobService;
        _tenantDbContext = tenantDbContext;
        _serviceProvider = serviceProvider;
        _tenantInfo = tenantInfo;
    }

    public async Task InitializeJobsForTenantAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Hangfire: Initializing Recurring Jobs");
        foreach (var tenant in await _tenantDbContext.TenantInfo.ToListAsync(cancellationToken))
        {
            InitializeJobsForTenant(tenant);
        }
    }

    public void InitializeJobsForTenant(TradeGeniusTenantInfo tenant)
    {
        // First create a new scope
        using var scope = _serviceProvider.CreateScope();

        // Then set current tenant so the right connectionstring is used
        scope.ServiceProvider.GetRequiredService<IMultiTenantContextAccessor>()
            .MultiTenantContext = new MultiTenantContext<TradeGeniusTenantInfo>()
            {
                TenantInfo = tenant
            };

        scope.ServiceProvider.GetRequiredService<IRecurringJobInitialization>()
            .InitializeRecurringJobs(tenant.Identifier);
    }

    public void InitializeRecurringJobs(string tenantId)
    {
        var interfaceType = typeof(IJobRecurringService);

        var interfaceTypes = AppDomain.CurrentDomain.GetAssemblies()
               .SelectMany(s => s.GetTypes())
               .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

        foreach (var type in interfaceTypes)
        {
            var implement = ActivatorUtilities.CreateInstance(_serviceProvider, type) as IJobRecurringService;

            Expression<Func<Task>> func = () => implement.CheckOut();

            _jobService.AddOrUpdate($"{tenantId}-{implement.Id}", func, () => implement.Time, implement.TimeZone, implement.Qoeue);

            _logger.LogInformation($"{tenantId}-{implement.Id}: All recurring jobs have been initialized.");
        }
    }
}