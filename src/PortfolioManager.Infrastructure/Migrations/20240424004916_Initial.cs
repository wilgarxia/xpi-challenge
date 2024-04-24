using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PortfolioManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    description = table.Column<string>(type: "text", nullable: false),
                    due_at = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    minimum_investment_amount = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    is_available = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    manager_email = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    lastname = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    is_admin = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "portfolio_product",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_portfolio_product", x => x.id);
                    table.ForeignKey(
                        name: "fk_portfolio_product_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_portfolio_product_users_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    direction = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transaction", x => x.id);
                    table.ForeignKey(
                        name: "fk_transaction_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_transaction_users_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "product",
                columns: new[] { "id", "created_at", "description", "due_at", "is_available", "manager_email", "minimum_investment_amount", "updated_at" },
                values: new object[,]
                {
                    { new Guid("6610e835-8a46-45dd-8b3d-2e3a64eaec6a"), new DateTime(2024, 1, 1, 3, 0, 0, 0, DateTimeKind.Utc), "XP BNP Multi Asset A.I. - Alta Alavancada", new DateTime(2029, 1, 1, 3, 0, 0, 0, DateTimeKind.Utc), true, "manager@mail.com", 5000m, null },
                    { new Guid("8ba38709-9633-4a93-9824-d4ee22951c26"), new DateTime(2024, 1, 1, 3, 0, 0, 0, DateTimeKind.Utc), "XP Debêntures Incentivadas Hedge CP FIC FIM LP", new DateTime(2029, 1, 1, 3, 0, 0, 0, DateTimeKind.Utc), true, "manager@mail.com", 1000m, null }
                });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "id", "created_at", "email", "first_name", "lastname", "password_hash" },
                values: new object[] { new Guid("0d706086-6829-45bc-ad95-b8b0d942aa84"), new DateTime(2024, 1, 1, 3, 0, 0, 0, DateTimeKind.Utc), "jane@example.com", "Jane", "Smith", "$2a$11$K4Pyu70lbon4I4bL25lFRuQRQHhjmbU3sb5kKrBr1sPLIbjmKSo66" });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "id", "created_at", "email", "first_name", "is_admin", "lastname", "password_hash" },
                values: new object[,]
                {
                    { new Guid("8d4e4aa3-0bdf-44d3-ba6a-e3383eacebc7"), new DateTime(2024, 1, 1, 3, 0, 0, 0, DateTimeKind.Utc), "alice@example.com", "Alice", true, "Johnson", "$2a$11$pZPzG980V7GoM2UNos3l9.onTf3trhSkwWT8gUshmcZfInY36NV8." },
                    { new Guid("aacbda5a-2add-469e-85b9-d14dff2eb38b"), new DateTime(2024, 1, 1, 3, 0, 0, 0, DateTimeKind.Utc), "john@example.com", "John", true, "Doe", "$2a$11$DsHUOKHXnObgR5.HCla.oegHc5SlhyeIkryF5gF.xFhWlTsWKnko6" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_portfolio_product_product_id",
                table: "portfolio_product",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_portfolio_product_user_id",
                table: "portfolio_product",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_transaction_product_id",
                table: "transaction",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_transaction_user_id",
                table: "transaction",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_email",
                table: "user",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "portfolio_product");

            migrationBuilder.DropTable(
                name: "transaction");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
