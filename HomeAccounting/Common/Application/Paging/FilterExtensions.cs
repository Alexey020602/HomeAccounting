using System.Linq.Expressions;
using HomeAccounting.Common.Model.ValueObjects;

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

            var processedString = Uri.UnescapeDataString(s);
            return
            [
                .. processedString.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(x => x.ParseFilterValue<TField>())
                    .Where(f => f is not null)
                    .Cast<Filter<TField>>()
            ];
        }

        /// <summary>
        /// Парсит строку с одним фильтром в объект Filter.
        /// Пустое значение интерпретируется как null и может использоваться с операторами Eq/Ne для проверки на null.
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

            if (left.ParseFieldNameWithOperator<TField, FilterOperator>() is not var (field, op))
                return null;

            // Разрешаем пустое значение - оно будет интерпретировано как null при применении фильтра
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

            if (propertyType == typeof(string))
            {
            }

            // Если значение пустое, интерпретируем его как null
            Expression constant;
            if (string.IsNullOrWhiteSpace(filter.Value))
            {
                // Для пустого значения используем null константу
                constant = Expression.Constant(null, propertyType);
            }
            else
            {
                var filterValue = ConvertValue(filter.Value, propertyType);
                constant = Expression.Constant(filterValue, propertyType);
            }

            Expression comparison = (filter.Operator, propertyType == typeof(string)) switch
            {
                (FilterOperator.Eq, _) => Expression.Equal(property, constant),
                (FilterOperator.Ne, _) => Expression.NotEqual(property, constant),
                (FilterOperator.Gt, _) => Expression.GreaterThan(property, constant),
                (FilterOperator.Gte, _) => Expression.GreaterThanOrEqual(property, constant),
                (FilterOperator.Lt, _) => Expression.LessThan(property, constant),
                (FilterOperator.Lte, _) => Expression.LessThanOrEqual(property, constant),
                (FilterOperator.Cn, true) => Expression.Contains(property, filter.Value),
                (FilterOperator.Nc, true) => Expression.NotContains(property, filter.Value),
                (FilterOperator.Sw, true) => Expression.StartsWith(property, filter.Value),
                (FilterOperator.Ew, true) => Expression.EndsWith(property, filter.Value),
                _ => filter.Operator.IsStringsOnly 
                    ? throw new NotSupportedException( $"Operator {filter.Operator} is not supported for not string types") 
                    : throw new NotSupportedException($"Operator {filter.Operator} is not supported") 
            };

            var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);

            return source.Where(lambda);
        }
    }

    extension(FilterOperator filterOperator)
    {
        public bool IsStringsOnly => filterOperator switch
        {
            (FilterOperator.Cn) => true,
            (FilterOperator.Nc) => true,
            (FilterOperator.Sw) => true,
            (FilterOperator.Ew) => true,
            _ => false,
        };
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

    extension(Expression)
    {
        public static Expression Contains(Expression left, string filterValue) =>
            FilterExpression(left, nameof(string.Contains), filterValue);

        public static Expression NotContains(Expression left, string filterValue) =>
            Expression.Not(Contains(left, filterValue));

        public static Expression StartsWith(Expression left, string filterValue) =>
            FilterExpression(left, nameof(string.StartsWith), filterValue);

        public static Expression EndsWith(Expression left, string filterValue) =>
            FilterExpression(left, nameof(string.EndsWith), filterValue);

        private static Expression FilterExpression(Expression left, string filterName, string filterValue)
        {
            return Expression.Call(
                left,
                filterName,
                null,
                Expression.Constant(filterValue)
            );
        }
    }
}