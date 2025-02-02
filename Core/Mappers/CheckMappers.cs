using Core.Model;

namespace Core.Mappers;

public static class CheckMappers
{
    public static Check ConvertToCheck(this DataBase.Entities.Check check)
    {
        return new Check
        {
            Id = check.Id,
            PurchaseDate = check.PurchaseDate,
            AddedDate = check.AddedDate,
            Products = check.Products.ConvertToProducts().ToList()
        };
    }

    public static IEnumerable<Check> ConvertToChecks(this IEnumerable<DataBase.Entities.Check> checks)
    {
        return checks.Select(ConvertToCheck);
    }
}