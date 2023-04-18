﻿using Finbuckle.MultiTenant.EntityFrameworkCore;
using TradeGenius.WebApi.Infrastructure.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TradeGenius.WebApi.Infrastructure.Persistence.Configuration;

public class AuditTrailConfig : IEntityTypeConfiguration<Trail>
{
    public void Configure(EntityTypeBuilder<Trail> builder) =>
        builder
            .ToTable("AuditTrails", SchemaNames.Auditing)
            .IsMultiTenant();
}