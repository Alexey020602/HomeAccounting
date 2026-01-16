using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBudgets.Budgets.Data.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "budgets");

            migrationBuilder.CreateSequence(
                name: "BudgetRoleSequence",
                schema: "budgets",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "CategoriesSequence",
                schema: "budgets",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "ProductsSequence",
                schema: "budgets",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "BudgetRoles",
                schema: "budgets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Permissions = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                schema: "budgets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BeginOfPeriod = table.Column<int>(type: "integer", nullable: false),
                    Limit = table.Column<int>(type: "integer", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "budgets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentCategoryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetUsers",
                schema: "budgets",
                columns: table => new
                {
                    BudgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BudgetRoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetUsers", x => new { x.UserId, x.BudgetId });
                    table.ForeignKey(
                        name: "FK_BudgetUsers_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalSchema: "budgets",
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Spendings",
                schema: "budgets",
                columns: table => new
                {
                    BudgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    Sum = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Fn = table.Column<string>(type: "text", nullable: true),
                    Fd = table.Column<string>(type: "text", nullable: true),
                    Fp = table.Column<string>(type: "text", nullable: true),
                    PurchasePlace = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spendings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spendings_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalSchema: "budgets",
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "budgets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Sum = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    ReceiptSpendingId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Spendings_ReceiptSpendingId",
                        column: x => x.ReceiptSpendingId,
                        principalSchema: "budgets",
                        principalTable: "Spendings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetUsers_BudgetId",
                schema: "budgets",
                table: "BudgetUsers",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ReceiptSpendingId",
                schema: "budgets",
                table: "Product",
                column: "ReceiptSpendingId");

            migrationBuilder.CreateIndex(
                name: "IX_Spendings_BudgetId",
                schema: "budgets",
                table: "Spendings",
                column: "BudgetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetRoles",
                schema: "budgets");

            migrationBuilder.DropTable(
                name: "BudgetUsers",
                schema: "budgets");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "budgets");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "budgets");

            migrationBuilder.DropTable(
                name: "Spendings",
                schema: "budgets");

            migrationBuilder.DropTable(
                name: "Budgets",
                schema: "budgets");

            migrationBuilder.DropSequence(
                name: "BudgetRoleSequence",
                schema: "budgets");

            migrationBuilder.DropSequence(
                name: "CategoriesSequence",
                schema: "budgets");

            migrationBuilder.DropSequence(
                name: "ProductsSequence",
                schema: "budgets");
        }
    }
}
