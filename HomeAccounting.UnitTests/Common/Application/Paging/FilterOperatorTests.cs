using HomeAccounting.Common.Application.Paging;

namespace HomeAccounting.UnitTests.Common.Application.Paging;

public class FilterOperatorTests
{
    [Fact]
    public void ParseFilterOperator_WithAllOperators_ParsesCorrectly()
    {
        // Arrange & Act & Assert
        Assert.Equal(FilterOperator.Equal, FilterOperator.Parse("eq"));
        Assert.Equal(FilterOperator.Equal, FilterOperator.Parse("EQ"));
        Assert.Equal(FilterOperator.NotEqual, FilterOperator.Parse("ne"));
        Assert.Equal(FilterOperator.GreaterThan, FilterOperator.Parse("gt"));
        Assert.Equal(FilterOperator.GreaterThanOrEqual, FilterOperator.Parse("gte"));
        Assert.Equal(FilterOperator.LessThan, FilterOperator.Parse("lt"));
        Assert.Equal(FilterOperator.LessThanOrEqual, FilterOperator.Parse("lte"));
    }

    [Fact]
    public void ParseFilterOperator_WithUnknownOperator_DefaultsToEqual()
    {
        // Arrange & Act
        var result = FilterOperator.Parse("unknown");

        // Assert
        Assert.Equal(FilterOperator.Equal, result);
    }
}