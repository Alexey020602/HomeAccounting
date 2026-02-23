using HomeAccounting.Common.Application.Paging;

namespace HomeAccounting.UnitTests.Common.Application.Paging;

public class SortingTests
{
    [Fact]
    public void ParseSorting_WithSingleSorting_ReturnsSingleSorting()
    {
        // Arrange
        const string input = "Name[Asc]";

        // Act
        var result = input.ParseSorting<TestField>();

        // Assert
        Assert.NotNull(result);
        var sortings = result.ToArray();
        Assert.Single(sortings);
        Assert.Equal(TestField.Name, sortings[0].Field);
        Assert.Equal(SortOrder.Asc, sortings[0].Order);
    }

    [Fact]
    public void ParseSorting_WithMultipleSortings_ReturnsAllSortings()
    {
        // Arrange
        const string input = "Name[Asc],Age[Desc],Status[Asc]";

        // Act
        var result = input.ParseSorting<TestField>();

        // Assert
        Assert.NotNull(result);
        var sortings = result.ToArray();
        Assert.Equal(3, sortings.Length);
        
        Assert.Equal(TestField.Name, sortings[0].Field);
        Assert.Equal(SortOrder.Asc, sortings[0].Order);
        
        Assert.Equal(TestField.Age, sortings[1].Field);
        Assert.Equal(SortOrder.Desc, sortings[1].Order);
        
        Assert.Equal(TestField.Status, sortings[2].Field);
        Assert.Equal(SortOrder.Asc, sortings[2].Order);
    }

    [Fact]
    public void ParseSorting_WithEmptyString_ReturnsNull()
    {
        // Arrange
        const string input = "";

        // Act
        var result = input.ParseSorting<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseSorting_WithWhitespaceOnly_ReturnsNull()
    {
        // Arrange
        const string input = "   ";

        // Act
        var result = input.ParseSorting<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseSorting_WithMixedValidAndInvalidSortings_ReturnsOnlyValidSortings()
    {
        // Arrange
        const string input = "Name[Asc],InvalidField[Desc],Age[Asc]";

        // Act
        var result = input.ParseSorting<TestField>();

        // Assert
        Assert.NotNull(result);
        var sortings = result.ToArray();
        Assert.Equal(2, sortings.Length);
        Assert.Equal(TestField.Name, sortings[0].Field);
        Assert.Equal(TestField.Age, sortings[1].Field);
    }

    [Fact]
    public void ParseSorting_WithSpacesAroundCommas_TrimsCorrectly()
    {
        // Arrange
        const string input = "Name[Asc] , Age[Desc] , Status[Asc]";

        // Act
        var result = input.ParseSorting<TestField>();

        // Assert
        Assert.NotNull(result);
        var sortings = result.ToArray();
        Assert.Equal(3, sortings.Length);
    }

    [Fact]
    public void ParseSorting_WithEmptySortingsBetweenCommas_SkipsEmptySortings()
    {
        // Arrange
        const string input = "Name[Asc],,Age[Desc]";

        // Act
        var result = input.ParseSorting<TestField>();

        // Assert
        Assert.NotNull(result);
        var sortings = result.ToArray();
        Assert.Equal(2, sortings.Length);
        Assert.Equal(TestField.Name, sortings[0].Field);
        Assert.Equal(TestField.Age, sortings[1].Field);
    }

    [Fact]
    public void ParseSorting_WithSortingsWithoutOrder_ReturnsSortingsWithDefaultOrder()
    {
        // Arrange
        const string input = "Name,Age";

        // Act
        var result = input.ParseSorting<TestField>();

        // Assert
        Assert.NotNull(result);
        var sortings = result.ToArray();
        Assert.Equal(2, sortings.Length);
        Assert.Equal(SortOrder.Asc, sortings[0].Order);
        Assert.Equal(SortOrder.Asc, sortings[1].Order);
    }
}
