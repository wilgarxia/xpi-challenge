using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PortfolioManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "manager",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    lastname = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_manager", x => x.id);
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
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    description = table.Column<string>(type: "text", nullable: false),
                    due_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    minimum_investment_amount = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    is_available = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    manager_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_manager_manager_id",
                        column: x => x.manager_id,
                        principalTable: "manager",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_user",
                columns: table => new
                {
                    products_id = table.Column<Guid>(type: "uuid", nullable: false),
                    users_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_user", x => new { x.products_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_product_user_products_products_id",
                        column: x => x.products_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_product_user_users_users_id",
                        column: x => x.users_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "manager",
                columns: new[] { "id", "created_at", "email", "first_name", "lastname" },
                values: new object[] { new Guid("700b3357-29a6-437c-ba7e-e31abae30049"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "gandalf@gmail.com", "Gandalf", "The Grey" });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "id", "created_at", "email", "first_name", "lastname", "password_hash" },
                values: new object[] { new Guid("0d706086-6829-45bc-ad95-b8b0d942aa84"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "frodo.baggins@mail.com", "Frodo", "Baggins", "$2a$11$Ytj6xRsTIEVcvti/PhFeUOcpKZ3O53Yp.5e74xka0lXb3/FhJdvwu" });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "id", "created_at", "email", "first_name", "is_admin", "lastname", "password_hash" },
                values: new object[] { new Guid("aacbda5a-2add-469e-85b9-d14dff2eb38b"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tom.bombadil@mail.com", "Tom", true, "Bombadil", "$2a$11$xfo0ikCN.paTU56KA3MR5ekr52..ps1wE2BiMPabUv2rnpQJSlyXK" });

            migrationBuilder.InsertData(
                table: "product",
                columns: new[] { "id", "created_at", "description", "due_at", "is_available", "manager_id", "minimum_investment_amount", "updated_at" },
                values: new object[,]
                {
                    { new Guid("6610e835-8a46-45dd-8b3d-2e3a64eaec6a"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "XP BNP Multi Asset A.I. - Alta Alavancada", new DateTime(2029, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new Guid("700b3357-29a6-437c-ba7e-e31abae30049"), 5000m, null },
                    { new Guid("8ba38709-9633-4a93-9824-d4ee22951c26"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "XP Debêntures Incentivadas Hedge CP FIC FIM LP", new DateTime(2029, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new Guid("700b3357-29a6-437c-ba7e-e31abae30049"), 1000m, null }
                });

            migrationBuilder.CreateIndex(
                name: "ix_product_manager_id",
                table: "product",
                column: "manager_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_user_users_id",
                table: "product_user",
                column: "users_id");

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
                name: "product_user");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "manager");
        }
    }
}
