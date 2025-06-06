namespace Fns.CheiCheck.Dto;

public class Root
{
    /// <summary>
    /// Статус запроса
    /// </summary>
    public required Status Status { get; init; }
    /// <summary>
    /// Результаты распознавания
    /// </summary>
    public required IReadOnlyList<Item> Items { get; init; }
    
}