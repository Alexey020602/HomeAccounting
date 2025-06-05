using System.Text.Json.Serialization;
using Checks.Contracts;
using Core.Model.Report;

namespace Core.Reports.Model;

public sealed class Report
{
    public required DateRange DateRange { get; init; }
    public required IReadOnlyList<Category> Categories { get; init; }
    [JsonIgnore]
    public int PennySum => Categories.Sum(c => c.PennySum);
    public Sum Sum => new Sum(PennySum);
}