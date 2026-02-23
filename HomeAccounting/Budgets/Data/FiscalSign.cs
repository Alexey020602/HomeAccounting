namespace HomeAccounting.Budgets.Data;

/// <summary>
/// Фискальный признак документа (ФП).
/// Должен содержать от 8 до 10 цифр.
/// </summary>
internal readonly record struct FiscalSign
{
    public string Value { get; }
    
    private FiscalSign(string value)
    {
        Value = value;
    }
    
    /// <summary>
    /// Creates a fiscal sign (FP) with validation.
    /// </summary>
    /// <param name="value">Fiscal sign string (must be 8-10 digits)</param>
    /// <returns>Validated FiscalSign</returns>
    /// <exception cref="ArgumentException">Thrown when value is invalid</exception>
    public static FiscalSign Create(string value)
    {
        if (value.Length is > 10 or < 8)
        {
            throw new ArgumentException("Fiscal sign (FP) must be between 8 and 10 digits", nameof(value));
        }
        
        if (!value.All(char.IsDigit))
        {
            throw new ArgumentException("Fiscal sign (FP) must contain only digits", nameof(value));
        }
        
        return new FiscalSign(value);
    }
    
    public override string ToString() => Value;
    
    /// <summary>
    /// Implicit conversion to string for convenience.
    /// </summary>
    public static implicit operator string(FiscalSign fp) => fp.Value;
}
