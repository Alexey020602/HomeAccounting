namespace HomeAccounting.Budgets.Data;

/// <summary>
/// Порядковый номер фискального документа (ФД).
/// Должен содержать от 3 до 5 цифр.
/// </summary>
internal readonly record struct FiscalDocument
{
    public string Value { get; }
    
    private FiscalDocument(string value)
    {
        Value = value;
    }
    
    /// <summary>
    /// Creates a fiscal document number (FD) with validation.
    /// </summary>
    /// <param name="value">Fiscal document string (must be 3-5 digits)</param>
    /// <returns>Validated FiscalDocument</returns>
    /// <exception cref="ArgumentException">Thrown when value is invalid</exception>
    public static FiscalDocument Create(string value)
    {
        if (value.Length is > 5 or < 3)
        {
            throw new ArgumentException("Fiscal document (FD) must be between 3 and 5 digits", nameof(value));
        }
        
        if (!value.All(char.IsDigit))
        {
            throw new ArgumentException("Fiscal document (FD) must contain only digits", nameof(value));
        }
        
        return new FiscalDocument(value);
    }
    
    public override string ToString() => Value;
    
    /// <summary>
    /// Implicit conversion to string for convenience.
    /// </summary>
    public static implicit operator string(FiscalDocument fd) => fd.Value;
}
