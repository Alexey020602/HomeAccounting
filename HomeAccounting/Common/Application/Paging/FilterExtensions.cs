using System.Linq.Expressions;
using ClientServerShared.Model.Money;

namespace HomeAccounting.Common.Application.Paging;

public static class FilterExtensions
{
    extension(string? s) 
    {
        /// <summary>
        /// Парсит строку с несколькими фильтрами, разделенными запятыми, в коллекцию объектов Filter.
        /// </summary>
        /// <typeparam name="TField">Тип enum для полей фильтрации.</typeparam>
        /// <returns>Коллекция объектов Filter, если парсинг выполнен успешно; в противном случае — null.</returns>
        public IEnumerable<Filter<TField>>? ParseFilter<TField>() where TField : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(s)) return null;

            return
            [
                .. s.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(x => x.ParseFilterValue<TField>())
                    .Where(f => f is not null)
                    .Cast<Filter<TField>>()
            ];
        }

        /// <summary>
        /// Парсит строку с одним фильтром в объект Filter.
        /// </summary>
        /// <typeparam name="TField">Тип enum для полей фильтрации.</typeparam>
        /// <returns>Объект Filter, если парсинг выполнен успешно; в противном случае — null.</returns>
        public Filter<TField>? ParseFilterValue<TField>() where TField : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            
            var equalsIndex = s.IndexOf('=');
            if (equalsIndex < 0)
                return null;

            var left = s[..equalsIndex].Trim();
            var value = s[(equalsIndex + 1)..].Trim();

            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (left.ParseFieldNameWithOperator<TField, FilterOperator>() is not var (field, op))
                return null;

            return new Filter<TField>(field, op, value);
        }
    }

    extension<T>(IQueryable<T> source)
    {
        public IQueryable<T> ApplyFilters<TField>(IEnumerable<Filter<TField>>? filters) where TField : Enum
        {
            if (filters is null || !filters.Any()) return source;

            foreach (var filter in filters)
            {
                source = source.ApplyFilter(filter);
            }
            
            return source;
        }
        public IQueryable<T> ApplyFilter<TField>(Filter<TField> filter) where TField : Enum
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, filter.Field.ToString());
            var propertyType = property.Type;
            
            var filterValue = ConvertValue(filter.Value, propertyType);
            var constant = Expression.Constant(filterValue, propertyType);
            
            Expression comparison = filter.Operator switch
            {
                FilterOperator.Eq => Expression.Equal(property, constant),
                FilterOperator.Ne => Expression.NotEqual(property, constant),
                FilterOperator.Gt => Expression.GreaterThan(property, constant),
                FilterOperator.Gte => Expression.GreaterThanOrEqual(property, constant),
                FilterOperator.Lt => Expression.LessThan(property, constant),
                FilterOperator.Lte => Expression.LessThanOrEqual(property, constant),
                _ => throw new NotSupportedException($"Operator {filter.Operator} is not supported"),
            };
            
            var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
            
            return source.Where(lambda);
        }

        
    }
    
    private static object? ConvertValue(string value, Type type)
    {
        // Обработка nullable типов
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
    
        // Обработка enum
        if (underlyingType.IsEnum)
        {
            return Enum.Parse(underlyingType, value, true);
        }
    
        // Обработка DateTimeOffset
        if (underlyingType == typeof(DateTimeOffset))
        {
            return DateTimeOffset.TryParse(value, out var dateTimeOffset) ? dateTimeOffset : null;
        }
    
        // Обработка Guid
        if (underlyingType == typeof(Guid))
        {
            return Guid.Parse(value);
        }
    
        // Обработка Money (кастомный тип)
        if (underlyingType == typeof(Money))
        {
            return Money.Parse(value);
        }
    
        // Обработка обычных типов через Convert.ChangeType
        return Convert.ChangeType(value, underlyingType);
    }
}