using System.Linq;
using HomeAccounting.Common.Application.Paging;

namespace HomeAccounting.UnitTests.Common.Application.Paging;

public class FilterApplyTests
{
    [Fact]
    public void ApplyFilter_WithEmptyValueAndEqOperator_FiltersNullValues()
    {
        // Arrange
        var source = new[]
        {
            new TestEntity { Name = "John", CompletedAt = null },
            new TestEntity { Name = "Jane", CompletedAt = DateTimeOffset.UtcNow },
            new TestEntity { Name = "Bob", CompletedAt = null }
        }.AsQueryable();

        var filter = new Filter<TestEntityField>(TestEntityField.CompletedAt, FilterOperator.Eq, "");

        // Act
        var result = source.ApplyFilter(filter).ToArray();

        // Assert
        Assert.Equal(2, result.Length);
        Assert.All(result, item => Assert.Null(item.CompletedAt));
    }

    [Fact]
    public void ApplyFilter_WithEmptyValueAndNeOperator_FiltersNonNullValues()
    {
        // Arrange
        var source = new[]
        {
            new TestEntity { Name = "John", CompletedAt = null },
            new TestEntity { Name = "Jane", CompletedAt = DateTimeOffset.UtcNow },
            new TestEntity { Name = "Bob", CompletedAt = DateTimeOffset.UtcNow.AddDays(-1) }
        }.AsQueryable();

        var filter = new Filter<TestEntityField>(TestEntityField.CompletedAt, FilterOperator.Ne, "");

        // Act
        var result = source.ApplyFilter(filter).ToArray();

        // Assert
        Assert.Equal(2, result.Length);
        Assert.All(result, item => Assert.NotNull(item.CompletedAt));
    }

    [Fact]
    public void ApplyFilter_WithEmptyValueAndEqOperator_WorksWithNullableInt()
    {
        // Arrange
        var source = new[]
        {
            new TestEntity { Name = "John", Age = null },
            new TestEntity { Name = "Jane", Age = 25 },
            new TestEntity { Name = "Bob", Age = null }
        }.AsQueryable();

        var filter = new Filter<TestEntityField>(TestEntityField.Age, FilterOperator.Eq, "");

        // Act
        var result = source.ApplyFilter(filter).ToArray();

        // Assert
        Assert.Equal(2, result.Length);
        Assert.All(result, item => Assert.Null(item.Age));
    }

    [Fact]
    public void ApplyFilter_WithEmptyValueAndNeOperator_WorksWithNullableInt()
    {
        // Arrange
        var source = new[]
        {
            new TestEntity { Name = "John", Age = null },
            new TestEntity { Name = "Jane", Age = 25 },
            new TestEntity { Name = "Bob", Age = 30 }
        }.AsQueryable();

        var filter = new Filter<TestEntityField>(TestEntityField.Age, FilterOperator.Ne, "");

        // Act
        var result = source.ApplyFilter(filter).ToArray();

        // Assert
        Assert.Equal(2, result.Length);
        Assert.All(result, item => Assert.NotNull(item.Age));
    }

    [Fact]
    public void ApplyFilter_WithNonEmptyValue_WorksAsBefore()
    {
        // Arrange
        var source = new[]
        {
            new TestEntity { Name = "John", Age = 25 },
            new TestEntity { Name = "Jane", Age = 30 },
            new TestEntity { Name = "Bob", Age = 25 }
        }.AsQueryable();

        var filter = new Filter<TestEntityField>(TestEntityField.Age, FilterOperator.Eq, "25");

        // Act
        var result = source.ApplyFilter(filter).ToArray();

        // Assert
        Assert.Equal(2, result.Length);
        Assert.All(result, item => Assert.Equal(25, item.Age));
    }

    [Fact]
    public void ApplyFilters_WithMultipleFiltersIncludingEmptyValue_AppliesAllFilters()
    {
        // Arrange
        var source = new[]
        {
            new TestEntity { Name = "John", Age = 25, CompletedAt = null },
            new TestEntity { Name = "Jane", Age = 25, CompletedAt = DateTimeOffset.UtcNow },
            new TestEntity { Name = "Bob", Age = 30, CompletedAt = null }
        }.AsQueryable();

        var filters = new[]
        {
            new Filter<TestEntityField>(TestEntityField.Age, FilterOperator.Eq, "25"),
            new Filter<TestEntityField>(TestEntityField.CompletedAt, FilterOperator.Eq, "")
        };

        // Act
        var result = source.ApplyFilters(filters).ToArray();

        // Assert
        Assert.Single(result);
        Assert.Equal("John", result[0].Name);
        Assert.Equal(25, result[0].Age);
        Assert.Null(result[0].CompletedAt);
    }
}

// Test entity for ApplyFilter tests
public class TestEntity
{
    public string Name { get; set; } = string.Empty;
    public int? Age { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
}

public enum TestEntityField
{
    Name,
    Age,
    CompletedAt
}
