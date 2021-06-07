﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using mbDirect.Vault.Repo;

namespace mbDirect.Vault.Repo.Migrations
{
    [DbContext(typeof(VaultContext))]
    [Migration("20210606172321_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("mbDirect.Vault.Models.Data.Account", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountStatusCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("AddDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("BillingZipCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Expiration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InstrumentTypeCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoutingNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StatusDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.HasIndex("AccountStatusCode");

                    b.HasIndex("InstrumentTypeCode");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("mbDirect.Vault.Models.Data.AccountStatus", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("LastMaintDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Code");

                    b.ToTable("AccountStatuses");
                });

            modelBuilder.Entity("mbDirect.Vault.Models.Data.InstrumentType", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("LastMaintDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Code");

                    b.ToTable("InstrumentTypes");

                    b.HasData(
                        new
                        {
                            Code = "credit",
                            LastMaintDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Credit Card"
                        },
                        new
                        {
                            Code = "debit",
                            LastMaintDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Debit Card"
                        });
                });

            modelBuilder.Entity("mbDirect.Vault.Models.Data.Account", b =>
                {
                    b.HasOne("mbDirect.Vault.Models.Data.AccountStatus", null)
                        .WithMany()
                        .HasForeignKey("AccountStatusCode");

                    b.HasOne("mbDirect.Vault.Models.Data.InstrumentType", null)
                        .WithMany()
                        .HasForeignKey("InstrumentTypeCode");
                });
#pragma warning restore 612, 618
        }
    }
}