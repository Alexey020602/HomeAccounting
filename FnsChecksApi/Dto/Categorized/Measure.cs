namespace FnsChecksApi.Dto.Categorized;

public record Measure(
    string Type,
    string Name,
    object Value
);