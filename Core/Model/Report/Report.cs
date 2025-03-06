using System.Text.Json.Serialization;

namespace Core.Model.Report;

public sealed class Report
{
    public required int FirstDay { get; init; }
    public required int Month { get; init; }
    public required int Year { get; init; }
    public required IReadOnlyList<ChecksList.Category> Categories { get; init; }
    [JsonIgnore]
    public int PennySum => Categories.Sum(c => c.PennySum);
    public Sum Sum => new Sum(PennySum);
    [JsonIgnore]
    public DateTime PeriodDate => new DateTime(Year, Month, FirstDay);
    
}