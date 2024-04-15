using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
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
                columns: new[] { "id", "is_admin", "password_hash", "username" },
                values: new object[] { new Guid("1487690a-7737-40d0-b6f2-0d628bcea9a4"), true, "$2a$11$qj64ixqDcKLn7IzkznZApeXNSpZFW59Qs2AITPyWecaBZEazWIrrK", "admin" });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "id", "password_hash", "username" },
                values: new object[] { new Guid("fc76a0bc-18d6-4d8b-99cc-1552412921fb"), "$2a$11$T6bdoSz4f5bzBN6yWbXLuu91m2vvhnAGoYmcQXc3i55plFgDLWn66", "user1" });

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
