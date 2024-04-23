﻿// <auto-generated />
using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nomis.DataAccess.PostgreSql.Referral.Persistence;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nomis.DataAccess.PostgreSql.Referral.Persistence.Migrations
{
    [DbContext(typeof(ReferralDbContext))]
    partial class ReferralDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Referral")
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Nomis.Domain.Entities.Audit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AffectedColumns")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EntityName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NewValues")
                        .HasColumnType("text");

                    b.Property<string>("OldValues")
                        .HasColumnType("text");

                    b.Property<string>("PrimaryKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("AuditTrails", "Referral");
                });

            modelBuilder.Entity("Nomis.Domain.Referral.Entities.ReferralData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("LastModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ReferralLevel")
                        .HasColumnType("integer");

                    b.Property<Guid>("ReferredWalletId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ReferringWalletId")
                        .HasColumnType("uuid");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ReferredWalletId");

                    b.HasIndex("ReferringWalletId");

                    b.ToTable("ReferralDatas", "Referral");
                });

            modelBuilder.Entity("Nomis.Domain.Referral.Entities.ReferralWallet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("LastModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ReferralCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RewardId")
                        .HasColumnType("uuid");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.Property<string>("WalletAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ReferralWallets", "Referral");
                });

            modelBuilder.Entity("Nomis.Domain.Referral.Entities.RewardData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("LastModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastPaidTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("PaidAmount")
                        .HasColumnType("numeric");

                    b.Property<Guid>("RewardedWalletId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("numeric");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RewardedWalletId")
                        .IsUnique();

                    b.ToTable("RewardDatas", "Referral");
                });

            modelBuilder.Entity("Nomis.Domain.Referral.Entities.ReferralData", b =>
                {
                    b.HasOne("Nomis.Domain.Referral.Entities.ReferralWallet", "ReferredWallet")
                        .WithMany()
                        .HasForeignKey("ReferredWalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nomis.Domain.Referral.Entities.ReferralWallet", "ReferringWallet")
                        .WithMany("Referrals")
                        .HasForeignKey("ReferringWalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ReferredWallet");

                    b.Navigation("ReferringWallet");
                });

            modelBuilder.Entity("Nomis.Domain.Referral.Entities.RewardData", b =>
                {
                    b.HasOne("Nomis.Domain.Referral.Entities.ReferralWallet", "RewardedWallet")
                        .WithOne("Reward")
                        .HasForeignKey("Nomis.Domain.Referral.Entities.RewardData", "RewardedWalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RewardedWallet");
                });

            modelBuilder.Entity("Nomis.Domain.Referral.Entities.ReferralWallet", b =>
                {
                    b.Navigation("Referrals");

                    b.Navigation("Reward")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
