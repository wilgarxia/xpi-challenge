using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
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
                columns: new[] { "id", "created_at", "password_hash", "username" },
                values: new object[] { new Guid("0d706086-6829-45bc-ad95-b8b0d942aa84"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$Ytj6xRsTIEVcvti/PhFeUOcpKZ3O53Yp.5e74xka0lXb3/FhJdvwu", "my-user" });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "id", "created_at", "is_admin", "password_hash", "username" },
                values: new object[] { new Guid("aacbda5a-2add-469e-85b9-d14dff2eb38b"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "$2a$11$xfo0ikCN.paTU56KA3MR5ekr52..ps1wE2BiMPabUv2rnpQJSlyXK", "admin" });

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
