using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budgets.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Budgets");

            migrationBuilder.CreateTable(
                name: "BudgetRoles",
                schema: "Budgets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Permissions = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                schema: "Budgets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
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
                name: "Invitations",
                schema: "Budgets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BudgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetUsers",
                schema: "Budgets",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BudgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    BudgetRoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetUsers", x => new { x.UserId, x.BudgetId, x.BudgetRoleId });
                    table.ForeignKey(
                        name: "FK_BudgetUsers_BudgetRoles_BudgetRoleId",
                        column: x => x.BudgetRoleId,
                        principalSchema: "Budgets",
                        principalTable: "BudgetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetUsers_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalSchema: "Budgets",
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetRoles_Name",
                schema: "Budgets",
                table: "BudgetRoles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_Name",
                schema: "Budgets",
                table: "Budgets",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetUsers_BudgetId",
                schema: "Budgets",
                table: "BudgetUsers",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetUsers_BudgetRoleId",
                schema: "Budgets",
                table: "BudgetUsers",
                column: "BudgetRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetUsers",
                schema: "Budgets");

            migrationBuilder.DropTable(
                name: "Invitations",
                schema: "Budgets");

            migrationBuilder.DropTable(
                name: "BudgetRoles",
                schema: "Budgets");

            migrationBuilder.DropTable(
                name: "Budgets",
                schema: "Budgets");
        }
    }
}
