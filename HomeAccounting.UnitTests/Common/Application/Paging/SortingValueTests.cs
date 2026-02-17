using HomeAccounting.Common.Application.Paging;

namespace HomeAccounting.UnitTests.Common.Application.Paging;

public class SortingValueTests
{
    [Fact]
    public void ParseSortingValue_WithFieldNameOnly_ReturnsAscSorting()
    {
        // Arrange
        const string input = "Name";

        // Act
        var result = input.ParseSortingValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestField.Name, result.Field);
        Assert.Equal(SortOrder.Asc, result.Order);
    }

    [Fact]
    public void ParseSortingValue_WithAscOrder_ReturnsAscSorting()
    {
        // Arrange
        const string input = "Name[Asc]";

        // Act
        var result = input.ParseSortingValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestField.Name, result.Field);
        Assert.Equal(SortOrder.Asc, result.Order);
    }

    [Fact]
    public void ParseSortingValue_WithDescOrder_ReturnsDescSorting()
    {
        // Arrange
        const string input = "Age[Desc]";

        // Act
        var result = input.ParseSortingValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestField.Age, result.Field);
        Assert.Equal(SortOrder.Desc, result.Order);
    }

    [Fact]
    public void ParseSortingValue_WithCaseInsensitiveFieldName_ParsesCorrectly()
    {
        // Arrange
        const string input = "name[Asc]";

        // Act
        var result = input.ParseSortingValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestField.Name, result.Field);
    }

    [Fact]
    public void ParseSortingValue_WithCaseInsensitiveOrder_ParsesCorrectly()
    {
        // Arrange
        const string input = "Age[ASC]";

        // Act
        var result = input.ParseSortingValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(SortOrder.Asc, result.Order);
    }

    [Fact]
    public void ParseSortingValue_WithSpacesAroundFieldName_TrimsCorrectly()
    {
        // Arrange
        const string input = "  Name  [Asc]";

        // Act
        var result = input.ParseSortingValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestField.Name, result.Field);
    }

    [Fact]
    public void ParseSortingValue_WithSpacesAroundOrder_TrimsCorrectly()
    {
        // Arrange
        const string input = "Name[  Asc  ]";

        // Act
        var result = input.ParseSortingValue<TestField>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(SortOrder.Asc, result.Order);
    }

    [Fact]
    public void ParseSortingValue_WithEmptyString_ReturnsNull()
    {
        // Arrange
        const string input = "";

        // Act
        var result = input.ParseSortingValue<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseSortingValue_WithWhitespaceOnly_ReturnsNull()
    {
        // Arrange
        const string input = "   ";

        // Act
        var result = input.ParseSortingValue<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseSortingValue_WithUnknownFieldName_ReturnsNull()
    {
        // Arrange
        const string input = "UnknownField[Asc]";

        // Act
        var result = input.ParseSortingValue<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseSortingValue_WithUnknownOrder_ReturnsNull()
    {
        // Arrange
        const string input = "Name[Unknown]";

        // Act
        var result = input.ParseSortingValue<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseSortingValue_WithoutClosingBracket_ReturnsNull()
    {
        // Arrange
        const string input = "Name[Asc";

        // Act
        var result = input.ParseSortingValue<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseSortingValue_WithMultipleOpeningBrackets_ReturnsNull()
    {
        // Arrange
        const string input = "Name[[Asc]";

        // Act
        var result = input.ParseSortingValue<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseSortingValue_WithMoreThanTwoBracketParts_ReturnsNull()
    {
        // Arrange
        const string input = "Name[Asc][extra]";

        // Act
        var result = input.ParseSortingValue<TestField>();

        // Assert
        Assert.Null(result);
    }
}
