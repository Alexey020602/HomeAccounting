namespace HomeAccounting.Common.Application.Paging;

public sealed record Sorting<TField>(TField Field, SortOrder Order) where TField: Enum;