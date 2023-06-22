using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeGenius.WebApi.Domain.Common.Messages;
using TradeGenius.WebApi.Domain.PersistMessageProcessor;

namespace TradeGenius.WebApi.Infrastructure.Persistence.Configuration;
internal class PersistMessageConfig : IEntityTypeConfiguration<PersistMessage>
{
    public void Configure(EntityTypeBuilder<PersistMessage> builder)
    {
        builder
            .ToTable(nameof(PersistMessage), SchemaNames.Message)
            .IsMultiTenant();

        // // ref: https://learn.microsoft.com/en-us/ef/core/saving/concurrency?tabs=fluent-api
        builder.Property(r => r.Version).IsConcurrencyToken();

        builder.Property(x => x.DeliveryType)
            .HasDefaultValue(MessageDeliveryType.Outbox)
            .HasConversion(
                x => x.ToString(),
                x => (MessageDeliveryType)Enum.Parse(typeof(MessageDeliveryType), x));


        builder.Property(x => x.MessageStatus)
            .HasDefaultValue(MessageStatus.InProgress)
            .HasConversion(
                v => v.ToString(),
                v => (MessageStatus)Enum.Parse(typeof(MessageStatus), v));

    }
}
