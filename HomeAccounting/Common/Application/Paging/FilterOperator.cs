namespace HomeAccounting.Common.Application.Paging;

/// <summary>
/// Операторы сравнения для фильтрации данных.
/// </summary>
public enum FilterOperator
{
    /// <summary>
    /// Равенство.
    /// </summary>
    Eq,

    /// <summary>
    /// Неравенство.
    /// </summary>
    Ne,

    /// <summary>
    /// Меньше.
    /// </summary>
    Lt,

    /// <summary>
    /// Меньше или равно.
    /// </summary>
    Lte,

    /// <summary>
    /// Больше.
    /// </summary>
    Gt,

    /// <summary>
    /// Больше или равно.
    /// </summary>
    Gte,
}