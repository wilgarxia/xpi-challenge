﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PortfolioManager.Infrastructure.Persistence.Commom;

#nullable disable

namespace PortfolioManager.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PortfolioManager.Domain.InvestmentAggregate.Investment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<DateTime>("DueAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("due_at");

                    b.Property<bool>("IsAvailable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("is_available");

                    b.Property<decimal>("MinimumInvestmentAmount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric")
                        .HasDefaultValue(0m)
                        .HasColumnName("minimum_investment_amount");

                    b.Property<int>("Risk")
                        .HasColumnType("integer")
                        .HasColumnName("risk");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("updated_at");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_investment");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_investment_user_id");

                    b.ToTable("investment", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("8ba38709-9633-4a93-9824-d4ee22951c26"),
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "XP Debêntures Incentivadas Hedge CP FIC FIM LP",
                            DueAt = new DateTime(2029, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsAvailable = true,
                            MinimumInvestmentAmount = 1000m,
                            Risk = 1,
                            Type = 0,
                            UserId = new Guid("aacbda5a-2add-469e-85b9-d14dff2eb38b")
                        },
                        new
                        {
                            Id = new Guid("6610e835-8a46-45dd-8b3d-2e3a64eaec6a"),
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "XP BNP Multi Asset A.I. - Alta Alavancada",
                            DueAt = new DateTime(2029, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsAvailable = true,
                            MinimumInvestmentAmount = 5000m,
                            Risk = 2,
                            Type = 1,
                            UserId = new Guid("aacbda5a-2add-469e-85b9-d14dff2eb38b")
                        });
                });

            modelBuilder.Entity("PortfolioManager.Domain.UserAggregate.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsAdmin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_admin");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_user");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasDatabaseName("ix_user_username");

                    b.ToTable("user", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("aacbda5a-2add-469e-85b9-d14dff2eb38b"),
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsAdmin = true,
                            PasswordHash = "$2a$11$xfo0ikCN.paTU56KA3MR5ekr52..ps1wE2BiMPabUv2rnpQJSlyXK",
                            Username = "admin"
                        },
                        new
                        {
                            Id = new Guid("0d706086-6829-45bc-ad95-b8b0d942aa84"),
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsAdmin = false,
                            PasswordHash = "$2a$11$Ytj6xRsTIEVcvti/PhFeUOcpKZ3O53Yp.5e74xka0lXb3/FhJdvwu",
                            Username = "my-user"
                        });
                });

            modelBuilder.Entity("PortfolioManager.Domain.InvestmentAggregate.Investment", b =>
                {
                    b.HasOne("PortfolioManager.Domain.UserAggregate.User", "Manager")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_investment_users_user_id");

                    b.Navigation("Manager");
                });
#pragma warning restore 612, 618
        }
    }
}