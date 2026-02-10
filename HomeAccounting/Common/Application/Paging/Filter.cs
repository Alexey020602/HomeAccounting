namespace HomeAccounting.Common.Application.Paging;

public record Filter<TField>(TField Field, FilterOperator Operator, string Value) where TField: Enum;