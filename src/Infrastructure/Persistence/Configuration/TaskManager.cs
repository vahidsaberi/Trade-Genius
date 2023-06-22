using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Infrastructure.Persistence.Configuration;

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .ToTable(nameof(Category), SchemaNames.DutyManager)
            .IsMultiTenant();

        builder
            .Property(b => b.Name)
                .HasMaxLength(256);
    }
}

public class StatusConfig : IEntityTypeConfiguration<Status>
{
    public void Configure(EntityTypeBuilder<Status> builder)
    {
        builder
            .ToTable(nameof(Status), SchemaNames.DutyManager)
            .IsMultiTenant();

        builder
            .Property(b => b.Name)
                .HasMaxLength(256);
    }
}

public class TaskConfig : IEntityTypeConfiguration<Domain.TaskManager.Task>
{
    public void Configure(EntityTypeBuilder<Domain.TaskManager.Task> builder)
    {
        builder
            .ToTable(nameof(Domain.TaskManager.Task), SchemaNames.DutyManager)
            .IsMultiTenant();

        builder
            .Property(b => b.Title)
                .HasMaxLength(1024);
    }
}

public class TaskHistoryConfig : IEntityTypeConfiguration<TaskHistory>
{
    public void Configure(EntityTypeBuilder<TaskHistory> builder)
    {
        builder
            .ToTable(nameof(TaskHistory), SchemaNames.DutyManager)
            .IsMultiTenant();
    }
}
