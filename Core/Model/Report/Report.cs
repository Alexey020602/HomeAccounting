using System.Text.Json.Serialization;

namespace Core.Model.Report;

public sealed class Report
{
    public required DateRange DateRange { get; init; }
    public required IReadOnlyList<ChecksList.Category> Categories { get; init; }
    [JsonIgnore]
    public int PennySum => Categories.Sum(c => c.PennySum);
    public Sum Sum => new Sum(PennySum);
}