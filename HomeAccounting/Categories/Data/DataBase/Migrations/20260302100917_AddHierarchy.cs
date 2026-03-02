using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAccounting.Categories.Data.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hierarchy",
                schema: "categories",
                table: "Categories",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
with recursive path_cte AS (
    select 
        c.""Id"", 
        c.""ParentCategoryId"",
        '/' AS path,
        Array[c.""Id""] AS Visited
    from categories.""Categories"" c
    where ""ParentCategoryId"" IS NULL
    
    UNION ALL 
    
    select 
        c.""Id"", 
        c.""ParentCategoryId"", 
        p.path || p.""Id""::text || '/',
        p.Visited || c.""Id"" as Visited
    from categories.""Categories"" c
    inner join path_cte p ON c.""ParentCategoryId"" = p.""Id""
    where c.""Id"" <> ALL (p.Visited)
)
update categories.""Categories"" c
set ""Hierarchy"" = path_cte.path
from path_cte 
where c.""Id"" = path_cte.""Id""
                                    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hierarchy",
                schema: "categories",
                table: "Categories");
        }
    }
}
