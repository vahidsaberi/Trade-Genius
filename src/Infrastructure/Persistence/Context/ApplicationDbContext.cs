using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TradeGenius.WebApi.Application.Common.Events;
using TradeGenius.WebApi.Application.Common.Interfaces;
using TradeGenius.WebApi.Domain.Catalog;
using TradeGenius.WebApi.Domain.TaskManager;
using TradeGenius.WebApi.Infrastructure.Persistence.Configuration;

namespace TradeGenius.WebApi.Infrastructure.Persistence.Context;

public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(ITenantInfo currentTenant, DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, IEventPublisher events)
        : base(currentTenant, options, currentUser, serializer, dbSettings, events)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Status> Statuses => Set<Status>();
    public DbSet<Domain.TaskManager.Task> Tasks => Set<Domain.TaskManager.Task>();
    public DbSet<TaskHistory> TaskHistories => Set<TaskHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Category>().ToTable("Categories", SchemaNames.DutyManager);
        //modelBuilder.Entity<Status>().ToTable("DutyStatuses", SchemaNames.DutyManager);
        //modelBuilder.Entity<Domain.TaskManager.Task>().ToTable("Duties", SchemaNames.DutyManager);
        //modelBuilder.Entity<TaskHistory>().ToTable("DutyHistories", SchemaNames.DutyManager);

        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.Catalog);

    }
}