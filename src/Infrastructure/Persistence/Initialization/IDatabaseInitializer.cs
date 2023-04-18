using TradeGenius.WebApi.Infrastructure.Multitenancy;

namespace TradeGenius.WebApi.Infrastructure.Persistence.Initialization;

internal interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
    Task InitializeApplicationDbForTenantAsync(TradeGeniusTenantInfo tenant, CancellationToken cancellationToken);
}