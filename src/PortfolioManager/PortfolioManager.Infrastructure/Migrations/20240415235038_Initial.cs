using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace PortfolioManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<ZonedDateTime>(type: "timestamp with time zone", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    is_admin = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "id", "created_at", "is_admin", "password_hash", "username" },
                values: new object[] { new Guid("02eb13a8-4d27-4eb4-89f6-7b7ee31c981c"), new NodaTime.ZonedDateTime(NodaTime.Instant.FromUnixTimeTicks(17132250372819489L), NodaTime.TimeZones.TzdbDateTimeZoneSource.Default.ForId("UTC")), true, "$2a$11$mss.4ov4tduZDhiOPAIs8OXWNSqxndvePKDlp6C3q4RbgxQw4U.tu", "admin" });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "id", "created_at", "password_hash", "username" },
                values: new object[] { new Guid("29e7df09-7f8d-468f-9184-630d18f0bc6d"), new NodaTime.ZonedDateTime(NodaTime.Instant.FromUnixTimeTicks(17132250375130368L), NodaTime.TimeZones.TzdbDateTimeZoneSource.Default.ForId("UTC")), "$2a$11$O0hM54kXoX3soYPh2neUUezHdjAWrdj1jFx8eFOSeKcLbo0BGFub6", "user1" });

            migrationBuilder.CreateIndex(
                name: "ix_user_username",
                table: "user",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
