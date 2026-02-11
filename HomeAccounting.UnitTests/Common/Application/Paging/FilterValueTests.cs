using HomeAccounting.Common.Application.Paging;

namespace HomeAccounting.UnitTests.Common.Application.Paging;

public class FilterValueTests
{
    [Fact]
    public void ParseFilterValue_WithFieldNameOnly_ReturnsEqualFilter()
    {
        // Arrange
        const string input = "Name=John";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestField.Name, result.Field);
        Assert.Equal(FilterOperator.Eq, result.Operator);
        Assert.Equal("John", result.Value);
    }

    [Fact]
    public void ParseFilterValue_WithEqOperator_ReturnsEqualFilter()
    {
        // Arrange
        const string input = "Name[eq]=John";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestField.Name, result.Field);
        Assert.Equal(FilterOperator.Eq, result.Operator);
        Assert.Equal("John", result.Value);
    }

    [Fact]
    public void ParseFilterValue_WithNeOperator_ReturnsNotEqualFilter()
    {
        // Arrange
        const string input = "Status[ne]=Active";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestField.Status, result.Field);
        Assert.Equal(FilterOperator.Ne, result.Operator);
        Assert.Equal("Active", result.Value);
    }

    [Fact]
    public void ParseFilterValue_WithGtOperator_ReturnsGreaterThanFilter()
    {
        // Arrange
        const string input = "Age[gt]=18";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestField.Age, result.Field);
        Assert.Equal(FilterOperator.Gt, result.Operator);
        Assert.Equal("18", result.Value);
    }

    [Fact]
    public void ParseFilterValue_WithGteOperator_ReturnsGreaterThanOrEqualFilter()
    {
        // Arrange
        const string input = "Age[gte]=18";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestField.Age, result.Field);
        Assert.Equal(FilterOperator.Gte, result.Operator);
        Assert.Equal("18", result.Value);
    }

    [Fact]
    public void ParseFilterValue_WithLtOperator_ReturnsLessThanFilter()
    {
        // Arrange
        const string input = "Age[lt]=65";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestField.Age, result.Field);
        Assert.Equal(FilterOperator.Lt, result.Operator);
        Assert.Equal("65", result.Value);
    }

    [Fact]
    public void ParseFilterValue_WithLteOperator_ReturnsLessThanOrEqualFilter()
    {
        // Arrange
        const string input = "Age[lte]=65";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestField.Age, result.Field);
        Assert.Equal(FilterOperator.Lte, result.Operator);
        Assert.Equal("65", result.Value);
    }

    [Fact]
    public void ParseFilterValue_WithCaseInsensitiveFieldName_ParsesCorrectly()
    {
        // Arrange
        const string input = "name[eq]=John";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestField.Name, result.Field);
    }

    [Fact]
    public void ParseFilterValue_WithCaseInsensitiveOperator_ParsesCorrectly()
    {
        // Arrange
        const string input = "Age[EQ]=18";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FilterOperator.Eq, result.Operator);
    }

    [Fact]
    public void ParseFilterValue_WithValueContainingEquals_ParsesCorrectly()
    {
        // Arrange
        const string input = "Name[eq]=John=Smith";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John=Smith", result.Value);
    }

    [Fact]
    public void ParseFilterValue_WithValueContainingMultipleEquals_ParsesCorrectly()
    {
        // Arrange
        const string input = "Name[eq]=a=b=c=d";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("a=b=c=d", result.Value);
    }

    [Fact]
    public void ParseFilterValue_WithSpacesAroundValue_TrimsCorrectly()
    {
        // Arrange
        const string input = "Name[eq]=  John  ";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result.Value);
    }

    [Fact]
    public void ParseFilterValue_WithSpacesAroundFieldName_TrimsCorrectly()
    {
        // Arrange
        const string input = "  Name  [eq]=John";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestField.Name, result.Field);
    }

    [Fact]
    public void ParseFilterValue_WithEmptyString_ReturnsNull()
    {
        // Arrange
        const string input = "";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseFilterValue_WithWhitespaceOnly_ReturnsNull()
    {
        // Arrange
        const string input = "   ";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseFilterValue_WithoutEqualsSign_ReturnsNull()
    {
        // Arrange
        const string input = "Name";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseFilterValue_WithEmptyValue_ReturnsNull()
    {
        // Arrange
        const string input = "Name[eq]=";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseFilterValue_WithWhitespaceOnlyValue_ReturnsNull()
    {
        // Arrange
        const string input = "Name[eq]=   ";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseFilterValue_WithEmptyFieldName_ReturnsNull()
    {
        // Arrange
        const string input = "[eq]=value";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseFilterValue_WithUnknownFieldName_ReturnsNull()
    {
        // Arrange
        const string input = "UnknownField[eq]=value";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseFilterValue_WithUnknownOperator_DefaultsToEqual()
    {
        // Arrange
        const string input = "Name[unknown]=John";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseFilterValue_WithoutClosingBracket_ReturnsNull()
    {
        // Arrange
        const string input = "Name[eq=John";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseFilterValue_WithMultipleOpeningBrackets_ReturnsNull()
    {
        // Arrange
        const string input = "Name[[eq]=value";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseFilterValue_WithMoreThanTwoBracketParts_ReturnsNull()
    {
        // Arrange
        const string input = "Name[eq][extra]=value";

        // Act
        var result = input.ParseFilterValue<TestField>();

        // Assert
        Assert.Null(result);
    }
}