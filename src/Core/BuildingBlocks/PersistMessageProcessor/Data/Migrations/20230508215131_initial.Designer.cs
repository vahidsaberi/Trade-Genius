﻿// <auto-generated />
using System;
using BuildingBlocks.PersistMessageProcessor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BuildingBlocks.PersistMessageProcessor.Data.Migrations
{
    [DbContext(typeof(PersistMessageDbContext))]
    [Migration("20230508215131_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BuildingBlocks.PersistMessageProcessor.PersistMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("Data")
                        .HasColumnType("text")
                        .HasColumnName("data");

                    b.Property<string>("DataType")
                        .HasColumnType("text")
                        .HasColumnName("data_type");

                    b.Property<string>("DeliveryType")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Outbox")
                        .HasColumnName("delivery_type");

                    b.Property<string>("MessageStatus")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("InProgress")
                        .HasColumnName("message_status");

                    b.Property<int>("RetryCount")
                        .HasColumnType("integer")
                        .HasColumnName("retry_count");

                    b.Property<long>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("bigint")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_persist_message");

                    b.ToTable("persist_message", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
