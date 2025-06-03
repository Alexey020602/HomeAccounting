using Checks.DataBase.Entities;
using Checks.DataBase.Mappers;
using Category = Checks.DataBase.Entities.Category;

namespace HomeAccountingTests;
//Надо проверить группировку 
public class UnitTest1
{
    // UnitTest1()
    // {
    //     context = new ApplicationContext(
    //         new DbContextOptionsBuilder<ApplicationContext>()
    //             .UseInMemoryDatabase("Test")
    //             .Options
    //     );
    //     reportUseCase = new ReportUseCase(context);
    // }

    // private readonly ApplicationContext context;
    // private readonly ReportUseCase reportUseCase;

    [Fact]
    public void TestProductsGrouping()
    {
        var products = GetProducts();
        var categories = products.ConvertToCategories();
        
        Assert.Equal(3, categories.Count);
        Assert.Equal(1, categories[0].Subcategories.Count);
        Assert.Equal(2, categories[1].Subcategories.Count);
        Assert.Equal(3, categories[2].Subcategories.Count);
    }

    // private List<Model.Category> 
    private List<Product> GetProducts()
    {
        var products = new List<Product>();
        for (var i = 1; i <= 3; i++)
        {
            products.AddRange(
                GetProductsWithSubcategories(
                    i,
                    new Category
                    {
                        Id = i, 
                        Name = $"Category {i}"
                    })
            );
        }

        return products;
    }

    private List<Product> GetProductsWithSubcategories(int number, Category category)
    {
        var products = new List<Product>();
        for (int i = 1; i <= number; i++)
        {
            products.AddRange(GetProductsWithSubcategories(
                i, 
                category,
                new Subcategory
                {
                    Id = i * number,
                    Category = category,
                    Name = $"Subcategory {i} {category.Name}",
                }));
        }
        return products;
    }

    private List<Product> GetProductsWithSubcategories(int number, Category category, Subcategory subcategory)
    {
        var products = new List<Product>();
        for (int i = 1; i <= number; i++)
        {
            products.Add(
                new()
                {
                    Id = subcategory.Id * number,
                    Subcategory = subcategory,
                    Name = $"Product {i} {subcategory.Name}",
                }
            );
        }

        return products;
    }

    // private void SeedData()
    // {
    //     context.Products.AddRange(GetProducts());
    // }
    // private List<Product> 
}