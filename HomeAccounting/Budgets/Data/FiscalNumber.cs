namespace HomeAccounting.Budgets.Data;

/// <summary>
/// Заводской номер экземпляра фискального накопителя (ФН).
/// Должен содержать ровно 16 цифр.
/// </summary>
internal readonly record struct FiscalNumber
{
    public string Value { get; }
    
    private FiscalNumber(string value)
    {
        Value = value;
    }
    
    /// <summary>
    /// Creates a fiscal number (FN) with validation.
    /// </summary>
    /// <param name="value">Fiscal number string (must be exactly 16 digits)</param>
    /// <returns>Validated FiscalNumber</returns>
    /// <exception cref="ArgumentException">Thrown when value is invalid</exception>
    public static FiscalNumber Create(string value)
    {
        if (value.Length != 16)
        {
            throw new ArgumentException("Fiscal number (FN) must be exactly 16 digits", nameof(value));
        }
        
        if (!value.All(char.IsDigit))
        {
            throw new ArgumentException("Fiscal number (FN) must contain only digits", nameof(value));
        }
        
        return new FiscalNumber(value);
    }
    
    public override string ToString() => Value;
    
    /// <summary>
    /// Implicit conversion to string for convenience.
    /// </summary>
    public static implicit operator string(FiscalNumber fn) => fn.Value;
}
