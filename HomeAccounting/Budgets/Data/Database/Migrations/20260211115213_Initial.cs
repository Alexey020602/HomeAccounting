using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAccounting.Budgets.Data.Database.Migrations
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
                    Limit = table.Column<long>(type: "bigint", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptProcessingOutboxEntry",
                schema: "budgets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ReceiptId = table.Column<Guid>(type: "uuid", nullable: false),
                    NextRetryAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AttemptCount = table.Column<int>(type: "integer", nullable: false),
                    LastErrorMessage = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptProcessingOutboxEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Receipts",
                schema: "budgets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    BudgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Fn = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Fd = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Fp = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Sum = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PurchasePlace = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastErrorMessage = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetUser",
                schema: "budgets",
                columns: table => new
                {
                    BudgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BudgetRoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetUser", x => new { x.UserId, x.BudgetId });
                    table.ForeignKey(
                        name: "FK_BudgetUser_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalSchema: "budgets",
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operation",
                schema: "budgets",
                columns: table => new
                {
                    BudgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    PurchaseDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AddedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Sum = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operation_Budgets_BudgetId",
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Sum = table.Column<long>(type: "bigint", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    ReceiptId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Receipts_ReceiptId",
                        column: x => x.ReceiptId,
                        principalSchema: "budgets",
                        principalTable: "Receipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_Name",
                schema: "budgets",
                table: "Budgets",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetUser_BudgetId",
                schema: "budgets",
                table: "BudgetUser",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Operation_BudgetId",
                schema: "budgets",
                table: "Operation",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                schema: "budgets",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ReceiptId",
                schema: "budgets",
                table: "Product",
                column: "ReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptProcessingOutboxEntry_ReceiptId",
                schema: "budgets",
                table: "ReceiptProcessingOutboxEntry",
                column: "ReceiptId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptProcessingOutboxEntry_Status_NextRetryAt",
                schema: "budgets",
                table: "ReceiptProcessingOutboxEntry",
                columns: new[] { "Status", "NextRetryAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_Fd_Fn_Fp",
                schema: "budgets",
                table: "Receipts",
                columns: new[] { "Fd", "Fn", "Fp" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetRoles",
                schema: "budgets");

            migrationBuilder.DropTable(
                name: "BudgetUser",
                schema: "budgets");

            migrationBuilder.DropTable(
                name: "Operation",
                schema: "budgets");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "budgets");

            migrationBuilder.DropTable(
                name: "ReceiptProcessingOutboxEntry",
                schema: "budgets");

            migrationBuilder.DropTable(
                name: "Budgets",
                schema: "budgets");

            migrationBuilder.DropTable(
                name: "Receipts",
                schema: "budgets");

            migrationBuilder.DropSequence(
                name: "BudgetRoleSequence",
                schema: "budgets");
        }
    }
}
