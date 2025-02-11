namespace Core.Model.Report;

public sealed class Report
{
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public required IReadOnlyList<ChecksList.Category> Categories { get; init; }
    public int PennySum => Categories.Sum(c => c.PennySum);
    public Sum Sum => new Sum(PennySum);
}