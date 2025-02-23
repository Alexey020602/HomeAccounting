namespace FnsChecksApi.Dto.Categorized;

public record Root(
    Status Status,
    IReadOnlyList<Item> Items
);