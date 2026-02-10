using HomeAccounting.Common.Application.Paging;

namespace HomeAccounting.UnitTests.Common.Application.Paging;

public class FilterTests
{
    [Fact]
    public void ParseFilter_WithSingleFilter_ReturnsSingleFilter()
    {
        // Arrange
        const string input = "Name[eq]=John";

        // Act
        var result = input.ParseFilter<TestField>();

        // Assert
        Assert.NotNull(result);
        var filters = result.ToArray();
        Assert.Single(filters);
        Assert.Equal(TestField.Name, filters[0].Field);
        Assert.Equal(FilterOperator.Equal, filters[0].Operator);
        Assert.Equal("John", filters[0].Value);
    }

    [Fact]
    public void ParseFilter_WithMultipleFilters_ReturnsAllFilters()
    {
        // Arrange
        const string input = "Name[eq]=John,Age[gt]=18,Status[ne]=Inactive";

        // Act
        var result = input.ParseFilter<TestField>();

        // Assert
        Assert.NotNull(result);
        var filters = result.ToArray();
        Assert.Equal(3, filters.Length);
        
        Assert.Equal(TestField.Name, filters[0].Field);
        Assert.Equal(FilterOperator.Equal, filters[0].Operator);
        Assert.Equal("John", filters[0].Value);
        
        Assert.Equal(TestField.Age, filters[1].Field);
        Assert.Equal(FilterOperator.GreaterThan, filters[1].Operator);
        Assert.Equal("18", filters[1].Value);
        
        Assert.Equal(TestField.Status, filters[2].Field);
        Assert.Equal(FilterOperator.NotEqual, filters[2].Operator);
        Assert.Equal("Inactive", filters[2].Value);
    }

    [Fact]
    public void ParseFilter_WithEmptyString_ReturnsNull()
    {
        // Arrange
        const string input = "";

        // Act
        var result = input.ParseFilter<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseFilter_WithWhitespaceOnly_ReturnsNull()
    {
        // Arrange
        const string input = "   ";

        // Act
        var result = input.ParseFilter<TestField>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseFilter_WithMixedValidAndInvalidFilters_ReturnsOnlyValidFilters()
    {
        // Arrange
        const string input = "Name[eq]=John,InvalidField[eq]=value,Age[gt]=18";

        // Act
        var result = input.ParseFilter<TestField>();

        // Assert
        Assert.NotNull(result);
        var filters = result.ToArray();
        Assert.Equal(2, filters.Length);
        Assert.Equal(TestField.Name, filters[0].Field);
        Assert.Equal(TestField.Age, filters[1].Field);
    }

    [Fact]
    public void ParseFilter_WithSpacesAroundCommas_TrimsCorrectly()
    {
        // Arrange
        const string input = "Name[eq]=John , Age[gt]=18 , Status[ne]=Inactive";

        // Act
        var result = input.ParseFilter<TestField>();

        // Assert
        Assert.NotNull(result);
        var filters = result.ToArray();
        Assert.Equal(3, filters.Length);
    }

    [Fact]
    public void ParseFilter_WithEmptyFiltersBetweenCommas_SkipsEmptyFilters()
    {
        // Arrange
        const string input = "Name[eq]=John,,Age[gt]=18";

        // Act
        var result = input.ParseFilter<TestField>();

        // Assert
        Assert.NotNull(result);
        var filters = result.ToArray();
        Assert.Equal(2, filters.Length);
        Assert.Equal(TestField.Name, filters[0].Field);
        Assert.Equal(TestField.Age, filters[1].Field);
    }

    [Fact]
    public void ParseFilter_WithFiltersWithoutOperators_ReturnsFiltersWithDefaultOperator()
    {
        // Arrange
        const string input = "Name=John,Age=18";

        // Act
        var result = input.ParseFilter<TestField>();

        // Assert
        Assert.NotNull(result);
        var filters = result.ToArray();
        Assert.Equal(2, filters.Length);
        Assert.Equal(FilterOperator.Equal, filters[0].Operator);
        Assert.Equal(FilterOperator.Equal, filters[1].Operator);
    }

    [Fact]
    public void ParseFilter_WithComplexValues_ParsesCorrectly()
    {
        // Arrange
        const string input = "Name[eq]=John=Smith,Amount[gte]=100.50";

        // Act
        var result = input.ParseFilter<TestField>();

        // Assert
        Assert.NotNull(result);
        var filters = result.ToArray();
        Assert.Equal(2, filters.Length);
        Assert.Equal("John=Smith", filters[0].Value);
        Assert.Equal("100.50", filters[1].Value);
    }

}