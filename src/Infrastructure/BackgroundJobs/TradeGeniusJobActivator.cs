﻿using Finbuckle.MultiTenant;
using TradeGenius.WebApi.Infrastructure.Auth;
using TradeGenius.WebApi.Infrastructure.Common;
using TradeGenius.WebApi.Infrastructure.Multitenancy;
using TradeGenius.WebApi.Shared.Multitenancy;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;

namespace TradeGenius.WebApi.Infrastructure.BackgroundJobs;

public class TradeGeniusJobActivator : JobActivator
{
    private readonly IServiceScopeFactory _scopeFactory;

    public TradeGeniusJobActivator(IServiceScopeFactory scopeFactory) =>
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));

    public override JobActivatorScope BeginScope(PerformContext context) =>
        new Scope(context, _scopeFactory.CreateScope());

    private class Scope : JobActivatorScope, IServiceProvider
    {
        private readonly PerformContext _context;
        private readonly IServiceScope _scope;

        public Scope(PerformContext context, IServiceScope scope)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));

            ReceiveParameters();
        }

        private async void ReceiveParameters()
        {
            string tenantId = _context.GetJobParameter<string>(MultitenancyConstants.TenantIdName);

            if (tenantId is not null)
            {
                var tenantContext = _scope.ServiceProvider.GetRequiredService<TenantDbContext>();
                var tenantInfo = await tenantContext.TenantInfo.FindAsync(tenantId);

                if (tenantInfo is null)
                {
                    throw new InvalidOperationException("Tenant is not valid");
                }

                _scope.ServiceProvider.GetRequiredService<IMultiTenantContextAccessor>()
                    .MultiTenantContext = new MultiTenantContext<TradeGeniusTenantInfo>
                    {
                        TenantInfo = tenantInfo
                    };
            }

            string userId = _context.GetJobParameter<string>(QueryStringKeys.UserId);
            if (!string.IsNullOrEmpty(userId))
            {
                _scope.ServiceProvider.GetRequiredService<ICurrentUserInitializer>()
                    .SetCurrentUserId(userId);
            }
        }

        public override object Resolve(Type type) =>
            ActivatorUtilities.GetServiceOrCreateInstance(this, type);

        object? IServiceProvider.GetService(Type serviceType) =>
            serviceType == typeof(PerformContext)
                ? _context
                : _scope.ServiceProvider.GetService(serviceType);
    }
}