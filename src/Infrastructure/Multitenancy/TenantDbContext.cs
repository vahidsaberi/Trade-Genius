using Finbuckle.MultiTenant.Stores;
using TradeGenius.WebApi.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace TradeGenius.WebApi.Infrastructure.Multitenancy;

public class TenantDbContext : EFCoreStoreDbContext<TradeGeniusTenantInfo>
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TradeGeniusTenantInfo>().ToTable("Tenants", SchemaNames.MultiTenancy);
    }
}