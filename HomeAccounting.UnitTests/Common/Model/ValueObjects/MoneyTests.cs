using HomeAccounting.Common.Model.ValueObjects;

namespace HomeAccounting.UnitTests.Common.Model.ValueObjects;

public class MoneyTests
{
    #region Factory Methods

    [Fact]
    public void FromKopecks_WithPositiveValue_ReturnsCorrectMoney()
    {
        // Arrange & Act
        var money = Money.FromKopecks(12345);

        // Assert
        Assert.Equal(12345, money.Kopecks);
        Assert.Equal(123.45m, money.AmountRubles);
    }

    [Fact]
    public void FromKopecks_WithNegativeValue_ReturnsCorrectMoney()
    {
        // Arrange & Act
        var money = Money.FromKopecks(-5000);

        // Assert
        Assert.Equal(-5000, money.Kopecks);
        Assert.Equal(-50.00m, money.AmountRubles);
    }

    [Fact]
    public void FromKopecks_WithZero_ReturnsZero()
    {
        // Arrange & Act
        var money = Money.FromKopecks(0);

        // Assert
        Assert.Equal(0, money.Kopecks);
        Assert.Equal(0m, money.AmountRubles);
        Assert.True(money.IsZero);
    }

    [Fact]
    public void FromRubles_WithPositiveValue_ReturnsCorrectMoney()
    {
        // Arrange & Act
        var money = Money.FromRubles(123.45m);

        // Assert
        Assert.Equal(12345, money.Kopecks);
        Assert.Equal(123.45m, money.AmountRubles);
    }

    [Fact]
    public void FromRubles_WithNegativeValue_ReturnsCorrectMoney()
    {
        // Arrange & Act
        var money = Money.FromRubles(-50.00m);

        // Assert
        Assert.Equal(-5000, money.Kopecks);
        Assert.Equal(-50.00m, money.AmountRubles);
    }

    [Fact]
    public void FromRubles_WithZero_ReturnsZero()
    {
        // Arrange & Act
        var money = Money.FromRubles(0m);

        // Assert
        Assert.Equal(0, money.Kopecks);
        Assert.Equal(0m, money.AmountRubles);
        Assert.True(money.IsZero);
    }

    [Fact]
    public void FromRubles_WithFractionalPart_RoundsAwayFromZero()
    {
        // Arrange & Act
        var money1 = Money.FromRubles(123.456m);
        var money2 = Money.FromRubles(123.454m);

        // Assert
        Assert.Equal(12346, money1.Kopecks); // 0.456 -> rounds to 0.46
        Assert.Equal(12345, money2.Kopecks); // 0.454 -> rounds to 0.45
    }

    [Fact]
    public void FromRubles_WithNegativeFractionalPart_RoundsAwayFromZero()
    {
        // Arrange & Act
        var money1 = Money.FromRubles(-123.456m);
        var money2 = Money.FromRubles(-123.454m);

        // Assert
        Assert.Equal(-12346, money1.Kopecks); // -0.456 -> rounds to -0.46
        Assert.Equal(-12345, money2.Kopecks); // -0.454 -> rounds to -0.45
    }

    [Fact]
    public void Zero_ReturnsZeroMoney()
    {
        // Arrange & Act
        var money = Money.Zero;

        // Assert
        Assert.Equal(0, money.Kopecks);
        Assert.Equal(0m, money.AmountRubles);
        Assert.True(money.IsZero);
        Assert.False(money.IsNegative);
    }

    #endregion

    #region Properties

    [Fact]
    public void Kopecks_ReturnsCorrectValue()
    {
        // Arrange
        var money = Money.FromKopecks(12345);

        // Act & Assert
        Assert.Equal(12345, money.Kopecks);
    }

    [Fact]
    public void AmountRubles_ConvertsKopecksToRubles()
    {
        // Arrange
        var money = Money.FromKopecks(12345);

        // Act & Assert
        Assert.Equal(123.45m, money.AmountRubles);
    }

    [Fact]
    public void AmountRubles_WithSingleKopeck_ReturnsCorrectDecimal()
    {
        // Arrange
        var money = Money.FromKopecks(1);

        // Act & Assert
        Assert.Equal(0.01m, money.AmountRubles);
    }

    [Fact]
    public void IsZero_WithZero_ReturnsTrue()
    {
        // Arrange
        var money = Money.Zero;

        // Act & Assert
        Assert.True(money.IsZero);
    }

    [Fact]
    public void IsZero_WithPositiveValue_ReturnsFalse()
    {
        // Arrange
        var money = Money.FromKopecks(100);

        // Act & Assert
        Assert.False(money.IsZero);
    }

    [Fact]
    public void IsZero_WithNegativeValue_ReturnsFalse()
    {
        // Arrange
        var money = Money.FromKopecks(-100);

        // Act & Assert
        Assert.False(money.IsZero);
    }

    [Fact]
    public void IsNegative_WithNegativeValue_ReturnsTrue()
    {
        // Arrange
        var money = Money.FromKopecks(-100);

        // Act & Assert
        Assert.True(money.IsNegative);
    }

    [Fact]
    public void IsNegative_WithPositiveValue_ReturnsFalse()
    {
        // Arrange
        var money = Money.FromKopecks(100);

        // Act & Assert
        Assert.False(money.IsNegative);
    }

    [Fact]
    public void IsNegative_WithZero_ReturnsFalse()
    {
        // Arrange
        var money = Money.Zero;

        // Act & Assert
        Assert.False(money.IsNegative);
    }

    #endregion

    #region Methods

    [Fact]
    public void Abs_WithPositiveValue_ReturnsSameValue()
    {
        // Arrange
        var money = Money.FromKopecks(100);

        // Act
        var result = money.Abs();

        // Assert
        Assert.Equal(100, result.Kopecks);
        Assert.Equal(money, result);
    }

    [Fact]
    public void Abs_WithNegativeValue_ReturnsPositiveValue()
    {
        // Arrange
        var money = Money.FromKopecks(-100);

        // Act
        var result = money.Abs();

        // Assert
        Assert.Equal(100, result.Kopecks);
        Assert.False(result.IsNegative);
    }

    [Fact]
    public void Abs_WithZero_ReturnsZero()
    {
        // Arrange
        var money = Money.Zero;

        // Act
        var result = money.Abs();

        // Assert
        Assert.Equal(0, result.Kopecks);
        Assert.True(result.IsZero);
    }

    [Fact]
    public void CompareTo_WithEqualValues_ReturnsZero()
    {
        // Arrange
        var money1 = Money.FromKopecks(100);
        var money2 = Money.FromKopecks(100);

        // Act
        var result = money1.CompareTo(money2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareTo_WithGreaterValue_ReturnsPositive()
    {
        // Arrange
        var money1 = Money.FromKopecks(200);
        var money2 = Money.FromKopecks(100);

        // Act
        var result = money1.CompareTo(money2);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_WithLesserValue_ReturnsNegative()
    {
        // Arrange
        var money1 = Money.FromKopecks(100);
        var money2 = Money.FromKopecks(200);

        // Act
        var result = money1.CompareTo(money2);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareTo_WithZero_ReturnsCorrectSign()
    {
        // Arrange
        var positive = Money.FromKopecks(100);
        var zero = Money.Zero;
        var negative = Money.FromKopecks(-100);

        // Act & Assert
        Assert.True(positive.CompareTo(zero) > 0);
        Assert.Equal(0, zero.CompareTo(zero));
        Assert.True(negative.CompareTo(zero) < 0);
    }

    [Fact]
    public void ToString_FormatsWithTwoDecimalPlaces()
    {
        // Arrange
        var money = Money.FromKopecks(12345);

        // Act
        var result = money.ToString();

        // Assert
        Assert.Equal("123.45", result);
    }

    [Fact]
    public void ToString_WithSingleKopeck_FormatsCorrectly()
    {
        // Arrange
        var money = Money.FromKopecks(1);

        // Act
        var result = money.ToString();

        // Assert
        Assert.Equal("0.01", result);
    }

    [Fact]
    public void ToString_WithNegativeValue_FormatsCorrectly()
    {
        // Arrange
        var money = Money.FromKopecks(-5000);

        // Act
        var result = money.ToString();

        // Assert
        Assert.Equal("-50.00", result);
    }

    [Fact]
    public void ToString_WithZero_FormatsCorrectly()
    {
        // Arrange
        var money = Money.Zero;

        // Act
        var result = money.ToString();

        // Assert
        Assert.Equal("0.00", result);
    }

    [Fact]
    public void ToString_UsesInvariantCulture()
    {
        // Arrange
        var money = Money.FromKopecks(12345);

        // Act
        var result = money.ToString();

        // Assert - должна использоваться точка, а не запятая
        Assert.Contains(".", result);
        Assert.DoesNotContain(",", result);
    }

    #endregion

    #region Arithmetic Operators

    [Fact]
    public void Addition_WithPositiveValues_ReturnsSum()
    {
        // Arrange
        var a = Money.FromKopecks(100);
        var b = Money.FromKopecks(200);

        // Act
        var result = a + b;

        // Assert
        Assert.Equal(300, result.Kopecks);
    }

    [Fact]
    public void Addition_WithZero_ReturnsSameValue()
    {
        // Arrange
        var a = Money.FromKopecks(100);
        var zero = Money.Zero;

        // Act
        var result = a + zero;

        // Assert
        Assert.Equal(100, result.Kopecks);
        Assert.Equal(a, result);
    }

    [Fact]
    public void Addition_WithNegativeValues_ReturnsCorrectSum()
    {
        // Arrange
        var a = Money.FromKopecks(100);
        var b = Money.FromKopecks(-50);

        // Act
        var result = a + b;

        // Assert
        Assert.Equal(50, result.Kopecks);
    }

    [Fact]
    public void Addition_WithBothNegative_ReturnsNegativeSum()
    {
        // Arrange
        var a = Money.FromKopecks(-100);
        var b = Money.FromKopecks(-50);

        // Act
        var result = a + b;

        // Assert
        Assert.Equal(-150, result.Kopecks);
    }

    [Fact]
    public void Addition_WithOverflow_ThrowsOverflowException()
    {
        // Arrange
        var a = Money.FromKopecks(long.MaxValue);
        var b = Money.FromKopecks(1);

        // Act & Assert
        var exception = Assert.Throws<OverflowException>(() => a + b);
        Assert.Contains("Money addition overflow", exception.Message);
    }

    [Fact]
    public void Subtraction_WithPositiveValues_ReturnsDifference()
    {
        // Arrange
        var a = Money.FromKopecks(200);
        var b = Money.FromKopecks(100);

        // Act
        var result = a - b;

        // Assert
        Assert.Equal(100, result.Kopecks);
    }

    [Fact]
    public void Subtraction_FromZero_ReturnsNegative()
    {
        // Arrange
        var zero = Money.Zero;
        var b = Money.FromKopecks(100);

        // Act
        var result = zero - b;

        // Assert
        Assert.Equal(-100, result.Kopecks);
        Assert.True(result.IsNegative);
    }

    [Fact]
    public void Subtraction_WithNegativeValues_ReturnsCorrectDifference()
    {
        // Arrange
        var a = Money.FromKopecks(100);
        var b = Money.FromKopecks(-50);

        // Act
        var result = a - b;

        // Assert
        Assert.Equal(150, result.Kopecks);
    }

    [Fact]
    public void Subtraction_WithOverflow_ThrowsOverflowException()
    {
        // Arrange
        var a = Money.FromKopecks(long.MinValue);
        var b = Money.FromKopecks(1);

        // Act & Assert
        var exception = Assert.Throws<OverflowException>(() => a - b);
        Assert.Contains("Money subtraction overflow", exception.Message);
    }

    [Fact]
    public void UnaryMinus_WithPositiveValue_ReturnsNegative()
    {
        // Arrange
        var money = Money.FromKopecks(100);

        // Act
        var result = -money;

        // Assert
        Assert.Equal(-100, result.Kopecks);
        Assert.True(result.IsNegative);
    }

    [Fact]
    public void UnaryMinus_WithNegativeValue_ReturnsPositive()
    {
        // Arrange
        var money = Money.FromKopecks(-100);

        // Act
        var result = -money;

        // Assert
        Assert.Equal(100, result.Kopecks);
        Assert.False(result.IsNegative);
    }

    [Fact]
    public void UnaryMinus_WithZero_ReturnsZero()
    {
        // Arrange
        var money = Money.Zero;

        // Act
        var result = -money;

        // Assert
        Assert.Equal(0, result.Kopecks);
        Assert.True(result.IsZero);
    }

    [Fact]
    public void UnaryMinus_WithMinValue_ThrowsOverflowException()
    {
        // Arrange
        var money = Money.FromKopecks(long.MinValue);

        // Act & Assert
        var exception = Assert.Throws<OverflowException>(() => -money);
        Assert.Contains("Cannot negate Money.MinValue", exception.Message);
    }

    [Fact]
    public void Multiplication_WithPositiveFactor_ReturnsProduct()
    {
        // Arrange
        var money = Money.FromKopecks(100);

        // Act
        var result = money * 2.5m;

        // Assert
        Assert.Equal(250, result.Kopecks);
    }

    [Fact]
    public void Multiplication_WithNegativeFactor_ReturnsNegativeProduct()
    {
        // Arrange
        var money = Money.FromKopecks(100);

        // Act
        var result = money * -2m;

        // Assert
        Assert.Equal(-200, result.Kopecks);
        Assert.True(result.IsNegative);
    }

    [Fact]
    public void Multiplication_WithZero_ReturnsZero()
    {
        // Arrange
        var money = Money.FromKopecks(100);

        // Act
        var result = money * 0m;

        // Assert
        Assert.Equal(0, result.Kopecks);
        Assert.True(result.IsZero);
    }

    [Fact]
    public void Multiplication_WithFractionalFactor_RoundsAwayFromZero()
    {
        // Arrange
        var money = Money.FromKopecks(100);

        // Act
        var result1 = money * 1.555m; // 100 * 1.555 = 155.5 -> rounds to 156
        var result2 = money * 1.554m; // 100 * 1.554 = 155.4 -> rounds to 155

        // Assert
        Assert.Equal(156, result1.Kopecks);
        Assert.Equal(155, result2.Kopecks);
    }

    [Fact]
    public void Multiplication_WithNegativeFractionalFactor_RoundsAwayFromZero()
    {
        // Arrange
        var money = Money.FromKopecks(100);

        // Act
        var result1 = money * -1.555m; // 100 * -1.555 = -155.5 -> rounds to -156
        var result2 = money * -1.554m; // 100 * -1.554 = -155.4 -> rounds to -155

        // Assert
        Assert.Equal(-156, result1.Kopecks);
        Assert.Equal(-155, result2.Kopecks);
    }

    [Fact]
    public void Multiplication_Commutative_ReturnsSameResult()
    {
        // Arrange
        var money = Money.FromKopecks(100);
        var factor = 2.5m;

        // Act
        var result1 = money * factor;
        var result2 = factor * money;

        // Assert
        Assert.Equal(result1, result2);
    }

    [Fact]
    public void Multiplication_WithOverflow_ThrowsOverflowException()
    {
        // Arrange
        var money = Money.FromKopecks(long.MaxValue);
        var factor = 2m;

        // Act & Assert
        var exception = Assert.Throws<OverflowException>(() => money * factor);
        Assert.Contains("Money multiplication overflow", exception.Message);
    }

    [Fact]
    public void Division_WithPositiveDivisor_ReturnsQuotient()
    {
        // Arrange
        var money = Money.FromKopecks(200);

        // Act
        var result = money / 2m;

        // Assert
        Assert.Equal(100, result.Kopecks);
    }

    [Fact]
    public void Division_WithNegativeDivisor_ReturnsNegativeQuotient()
    {
        // Arrange
        var money = Money.FromKopecks(200);

        // Act
        var result = money / -2m;

        // Assert
        Assert.Equal(-100, result.Kopecks);
        Assert.True(result.IsNegative);
    }

    [Fact]
    public void Division_WithFractionalResult_RoundsAwayFromZero()
    {
        // Arrange
        var money = Money.FromKopecks(100);

        // Act
        var result1 = money / 3m; // 100 / 3 = 33.333... -> rounds to 33
        var result2 = Money.FromKopecks(200) / 3m; // 200 / 3 = 66.666... -> rounds to 67

        // Assert
        Assert.Equal(33, result1.Kopecks);
        Assert.Equal(67, result2.Kopecks);
    }

    [Fact]
    public void Division_ByZero_ThrowsDivideByZeroException()
    {
        // Arrange
        var money = Money.FromKopecks(100);

        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => money / 0m);
    }

    #endregion

    #region Comparison Operators

    [Fact]
    public void GreaterThan_WithGreaterValue_ReturnsTrue()
    {
        // Arrange
        var a = Money.FromKopecks(200);
        var b = Money.FromKopecks(100);

        // Act & Assert
        Assert.True(a > b);
    }

    [Fact]
    public void GreaterThan_WithLesserValue_ReturnsFalse()
    {
        // Arrange
        var a = Money.FromKopecks(100);
        var b = Money.FromKopecks(200);

        // Act & Assert
        Assert.False(a > b);
    }

    [Fact]
    public void GreaterThan_WithEqualValues_ReturnsFalse()
    {
        // Arrange
        var a = Money.FromKopecks(100);
        var b = Money.FromKopecks(100);

        // Act & Assert
        Assert.False(a > b);
    }

    [Fact]
    public void LessThan_WithLesserValue_ReturnsTrue()
    {
        // Arrange
        var a = Money.FromKopecks(100);
        var b = Money.FromKopecks(200);

        // Act & Assert
        Assert.True(a < b);
    }

    [Fact]
    public void LessThan_WithGreaterValue_ReturnsFalse()
    {
        // Arrange
        var a = Money.FromKopecks(200);
        var b = Money.FromKopecks(100);

        // Act & Assert
        Assert.False(a < b);
    }

    [Fact]
    public void LessThan_WithEqualValues_ReturnsFalse()
    {
        // Arrange
        var a = Money.FromKopecks(100);
        var b = Money.FromKopecks(100);

        // Act & Assert
        Assert.False(a < b);
    }

    [Fact]
    public void GreaterThanOrEqual_WithGreaterValue_ReturnsTrue()
    {
        // Arrange
        var a = Money.FromKopecks(200);
        var b = Money.FromKopecks(100);

        // Act & Assert
        Assert.True(a >= b);
    }

    [Fact]
    public void GreaterThanOrEqual_WithEqualValues_ReturnsTrue()
    {
        // Arrange
        var a = Money.FromKopecks(100);
        var b = Money.FromKopecks(100);

        // Act & Assert
        Assert.True(a >= b);
    }

    [Fact]
    public void GreaterThanOrEqual_WithLesserValue_ReturnsFalse()
    {
        // Arrange
        var a = Money.FromKopecks(100);
        var b = Money.FromKopecks(200);

        // Act & Assert
        Assert.False(a >= b);
    }

    [Fact]
    public void LessThanOrEqual_WithLesserValue_ReturnsTrue()
    {
        // Arrange
        var a = Money.FromKopecks(100);
        var b = Money.FromKopecks(200);

        // Act & Assert
        Assert.True(a <= b);
    }

    [Fact]
    public void LessThanOrEqual_WithEqualValues_ReturnsTrue()
    {
        // Arrange
        var a = Money.FromKopecks(100);
        var b = Money.FromKopecks(100);

        // Act & Assert
        Assert.True(a <= b);
    }

    [Fact]
    public void LessThanOrEqual_WithGreaterValue_ReturnsFalse()
    {
        // Arrange
        var a = Money.FromKopecks(200);
        var b = Money.FromKopecks(100);

        // Act & Assert
        Assert.False(a <= b);
    }
    #pragma warning disable CS1718
    [Fact]
    public void ComparisonOperators_WithZero_WorkCorrectly()
    {
        // Arrange
        var positive = Money.FromKopecks(100);
        var zero = Money.Zero;
        var negative = Money.FromKopecks(-100);

        // Act & Assert
        Assert.True(positive > zero);
        Assert.True(positive >= zero);
        Assert.False(positive < zero);
        Assert.False(positive <= zero);

        Assert.False(zero > zero);
        Assert.True(zero >= zero);
        Assert.False(zero < zero);
        Assert.True(zero <= zero);

        Assert.False(negative > zero);
        Assert.False(negative >= zero);
        Assert.True(negative < zero);
        Assert.True(negative <= zero);
    }
    #pragma warning restore CS1718
    [Fact]
    public void ComparisonOperators_WithNegativeValues_WorkCorrectly()
    {
        // Arrange
        var a = Money.FromKopecks(-100);
        var b = Money.FromKopecks(-200);

        // Act & Assert
        Assert.True(a > b); // -100 > -200
        Assert.True(a >= b);
        Assert.False(a < b);
        Assert.False(a <= b);
    }

    #endregion

    #region Parse

    [Fact]
    public void Parse_WithValidString_ReturnsCorrectMoney()
    {
        // Arrange
        const string input = "123.45";

        // Act
        var result = Money.Parse(input);

        // Assert
        Assert.Equal(12345, result.Kopecks);
        Assert.Equal(123.45m, result.AmountRubles);
    }

    [Fact]
    public void Parse_WithoutFractionalPart_ReturnsCorrectMoney()
    {
        // Arrange
        const string input = "15";

        // Act
        var result = Money.Parse(input);

        // Assert
        Assert.Equal(1500, result.Kopecks);
        Assert.Equal(15.00m, result.AmountRubles);
    }

    [Fact]
    public void Parse_WithNegativeValue_ReturnsCorrectMoney()
    {
        // Arrange
        const string input = "-10.00";

        // Act
        var result = Money.Parse(input);

        // Assert
        Assert.Equal(-1000, result.Kopecks);
        Assert.Equal(-10.00m, result.AmountRubles);
        Assert.True(result.IsNegative);
    }

    [Fact]
    public void Parse_WithThreeDecimalPlaces_RoundsAwayFromZero()
    {
        // Arrange
        const string input = "1.999";

        // Act
        var result = Money.Parse(input);

        // Assert
        Assert.Equal(200, result.Kopecks); // rounds to 2.00
        Assert.Equal(2.00m, result.AmountRubles);
    }

    [Fact]
    public void Parse_WithNegativeThreeDecimalPlaces_RoundsAwayFromZero()
    {
        // Arrange
        const string input = "-1.999";

        // Act
        var result = Money.Parse(input);

        // Assert
        Assert.Equal(-200, result.Kopecks); // rounds to -2.00
        Assert.Equal(-2.00m, result.AmountRubles);
    }

    [Fact]
    public void Parse_WithInvalidString_ThrowsFormatException()
    {
        // Arrange
        const string input = "invalid";

        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => Money.Parse(input));
        Assert.Contains("Invalid money format", exception.Message);
    }

    [Fact]
    public void Parse_WithEmptyString_ThrowsFormatException()
    {
        // Arrange
        const string input = "";

        // Act & Assert
        Assert.Throws<FormatException>(() => Money.Parse(input));
    }

    [Fact]
    public void Parse_WithWhitespaceOnly_ThrowsFormatException()
    {
        // Arrange
        const string input = "   ";

        // Act & Assert
        Assert.Throws<FormatException>(() => Money.Parse(input));
    }

    [Fact]
    public void Parse_WithOverflow_ThrowsFormatException()
    {
        // Arrange
        var input = (long.MaxValue / 100m + 1).ToString("F2");

        // Act & Assert
        Assert.Throws<FormatException>(() => Money.Parse(input));
    }

    #endregion

    #region TryParse

    [Fact]
    public void TryParse_WithValidString_ReturnsTrueAndCorrectMoney()
    {
        // Arrange
        const string input = "123.45";

        // Act
        var success = Money.TryParse(input, out var result);

        // Assert
        Assert.True(success);
        Assert.Equal(12345, result.Kopecks);
        Assert.Equal(123.45m, result.AmountRubles);
    }

    [Fact]
    public void TryParse_WithInvalidString_ReturnsFalse()
    {
        // Arrange
        const string input = "invalid";

        // Act
        var success = Money.TryParse(input, out var result);

        // Assert
        Assert.False(success);
        Assert.Equal(0, result.Kopecks);
    }

    [Fact]
    public void TryParse_WithNull_ReturnsFalse()
    {
        // Arrange
        string? input = null;

        // Act
        var success = Money.TryParse(input, out var result);

        // Assert
        Assert.False(success);
        Assert.Equal(0, result.Kopecks);
    }

    [Fact]
    public void TryParse_WithEmptyString_ReturnsFalse()
    {
        // Arrange
        const string input = "";

        // Act
        var success = Money.TryParse(input, out var result);

        // Assert
        Assert.False(success);
        Assert.Equal(0, result.Kopecks);
    }

    [Fact]
    public void TryParse_WithWhitespaceOnly_ReturnsFalse()
    {
        // Arrange
        const string input = "   ";

        // Act
        var success = Money.TryParse(input, out var result);

        // Assert
        Assert.False(success);
        Assert.Equal(0, result.Kopecks);
    }

    [Fact]
    public void TryParse_WithSpacesAroundValue_TrimsAndParses()
    {
        // Arrange
        const string input = "  123.45  ";

        // Act
        var success = Money.TryParse(input, out var result);

        // Assert
        Assert.True(success);
        Assert.Equal(12345, result.Kopecks);
    }

    [Fact]
    public void TryParse_UsesInvariantCulture()
    {
        // Arrange
        const string input = "123.45"; // точка как разделитель

        // Act
        var success = Money.TryParse(input, out var result);

        // Assert
        Assert.True(success);
        Assert.Equal(12345, result.Kopecks);
    }

    [Fact]
    public void TryParse_WithOverflow_ReturnsFalse()
    {
        // Arrange
        var input = (long.MaxValue / 100m + 1).ToString("F2");

        // Act
        var success = Money.TryParse(input, out var result);

        // Assert
        Assert.False(success);
        Assert.Equal(0, result.Kopecks);
    }

    [Fact]
    public void TryParse_WithNegativeValue_ReturnsTrue()
    {
        // Arrange
        const string input = "-10.00";

        // Act
        var success = Money.TryParse(input, out var result);

        // Assert
        Assert.True(success);
        Assert.Equal(-1000, result.Kopecks);
        Assert.True(result.IsNegative);
    }

    [Fact]
    public void TryParse_WithoutFractionalPart_ReturnsTrue()
    {
        // Arrange
        const string input = "15";

        // Act
        var success = Money.TryParse(input, out var result);

        // Assert
        Assert.True(success);
        Assert.Equal(1500, result.Kopecks);
        Assert.Equal(15.00m, result.AmountRubles);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void FromKopecks_WithMaxLongValue_WorksCorrectly()
    {
        // Arrange & Act
        var money = Money.FromKopecks(long.MaxValue);

        // Assert
        Assert.Equal(long.MaxValue, money.Kopecks);
        Assert.False(money.IsZero);
        Assert.False(money.IsNegative);
    }

    [Fact]
    public void FromKopecks_WithMinLongValue_WorksCorrectly()
    {
        // Arrange & Act
        var money = Money.FromKopecks(long.MinValue);

        // Assert
        Assert.Equal(long.MinValue, money.Kopecks);
        Assert.False(money.IsZero);
        Assert.True(money.IsNegative);
    }

    [Fact]
    public void Rounding_AwayFromZero_WorksCorrectly()
    {
        // Arrange & Act
        var money1 = Money.FromRubles(0.005m);
        var money2 = Money.FromRubles(-0.005m);
        var money3 = Money.FromRubles(0.004m);
        var money4 = Money.FromRubles(-0.004m);

        // Assert
        Assert.Equal(1, money1.Kopecks); // 0.5 -> rounds to 1
        Assert.Equal(-1, money2.Kopecks); // -0.5 -> rounds to -1
        Assert.Equal(0, money3.Kopecks); // 0.4 -> rounds to 0
        Assert.Equal(-0, money4.Kopecks); // -0.4 -> rounds to 0
    }

    [Fact]
    public void RecordStructEquality_WithSameValues_AreEqual()
    {
        // Arrange
        var money1 = Money.FromKopecks(100);
        var money2 = Money.FromKopecks(100);

        // Act & Assert
        Assert.Equal(money1, money2);
        Assert.True(money1 == money2);
        Assert.False(money1 != money2);
    }

    [Fact]
    public void RecordStructEquality_WithDifferentValues_AreNotEqual()
    {
        // Arrange
        var money1 = Money.FromKopecks(100);
        var money2 = Money.FromKopecks(200);

        // Act & Assert
        Assert.NotEqual(money1, money2);
        Assert.False(money1 == money2);
        Assert.True(money1 != money2);
    }

    [Fact]
    public void Precision_WhenConvertingRublesToKopecks_MaintainsAccuracy()
    {
        // Arrange & Act
        var money = Money.FromRubles(123.45m);

        // Assert
        Assert.Equal(12345, money.Kopecks);
        // Проверяем обратную конвертацию
        Assert.Equal(123.45m, money.AmountRubles);
    }

    #endregion
}
