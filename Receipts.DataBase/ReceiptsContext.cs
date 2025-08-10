using Microsoft.EntityFrameworkCore;
using Receipts.DataBase.Entities;

namespace Receipts.DataBase;

public class ReceiptsContext(DbContextOptions<ReceiptsContext> options)
    : DbContext(options)
{
    internal DbSet<Category> Categories { get; set; } = null!;
    internal DbSet<Product> Products { get; set; } = null!;
    internal DbSet<Subcategory> Subcategories { get; set; } = null!;
    internal DbSet<Check> Checks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(ReceiptsDbConstants.ShemaName);
    }
}

public static class ReceiptsDbConstants
{
    public const string? ShemaName = "Receipts";
}