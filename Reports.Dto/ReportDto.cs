using System.Text.Json.Serialization;
using Core.Reports.Model;
using Shared.Model;
using Shared.Model.Checks;

namespace Reports.Dto;

public sealed class ReportDto
{
    public required DateRange DateRange { get; init; }
    public required IReadOnlyList<CategoryDto> Categories { get; init; }
    [JsonIgnore]
    public int PennySum => Categories.Sum(c => c.PennySum);
    public Sum Sum => new Sum(PennySum);
}

public record CategoryDto(int Id, string Name, IReadOnlyList<SubcategoryDto> Subcategories)
{
    public int PennySum => Subcategories.Sum(s => s.PennySum);
}

public record SubcategoryDto(int Id, string Name, IReadOnlyList<ProductDto> Products)
{
    public int PennySum => Products.Sum(p => p.PennySum);
}
public record ProductDto(int Id, string? Name, double Quantity, int PennySum, int Price);