using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PortfolioManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInvestmentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "investment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    description = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    risk = table.Column<int>(type: "integer", nullable: false),
                    due_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    minimum_investment_amount = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    is_available = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_investment", x => x.id);
                    table.ForeignKey(
                        name: "fk_investment_users_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "investment",
                columns: new[] { "id", "created_at", "description", "due_at", "is_available", "minimum_investment_amount", "risk", "type", "updated_at", "user_id" },
                values: new object[,]
                {
                    { new Guid("6610e835-8a46-45dd-8b3d-2e3a64eaec6a"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "XP BNP Multi Asset A.I. - Alta Alavancada", new DateTime(2029, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 5000m, 2, 1, null, new Guid("aacbda5a-2add-469e-85b9-d14dff2eb38b") },
                    { new Guid("8ba38709-9633-4a93-9824-d4ee22951c26"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "XP Debêntures Incentivadas Hedge CP FIC FIM LP", new DateTime(2029, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1000m, 1, 0, null, new Guid("aacbda5a-2add-469e-85b9-d14dff2eb38b") }
                });

            migrationBuilder.CreateIndex(
                name: "ix_investment_user_id",
                table: "investment",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "investment");
        }
    }
}
