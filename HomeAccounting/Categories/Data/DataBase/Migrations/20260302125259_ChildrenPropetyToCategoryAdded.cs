using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAccounting.Categories.Data.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChildrenPropetyToCategoryAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                schema: "categories",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_ParentCategoryId",
                schema: "categories",
                table: "Categories",
                column: "ParentCategoryId",
                principalSchema: "categories",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Categories_ParentCategoryId",
                schema: "categories",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ParentCategoryId",
                schema: "categories",
                table: "Categories");
        }
    }
}
