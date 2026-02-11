using System.Linq.Expressions;

namespace HomeAccounting.Common.Application.Paging;

public static class SortingExtensions
{
    extension(string? s)
    {
        /// <summary>
        /// Парсит строку с несколькими сортировками, разделенными запятыми, в коллекцию объектов Sorting.
        /// </summary>
        /// <typeparam name="TField">Тип enum для полей сортировки.</typeparam>
        /// <returns>Коллекция объектов Sorting, если парсинг выполнен успешно; в противном случае — null.</returns>
        public IEnumerable<Sorting<TField>>? ParseSorting<TField>() where TField : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(s)) return null;

            return
            [
                .. s.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(x => x.ParseSortingValue<TField>())
                    .Where(f => f is not null)
                    .Cast<Sorting<TField>>()
            ];
        }

        /// <summary>
        /// Парсит строку с одной сортировкой в объект Sorting.
        /// </summary>
        /// <typeparam name="TField">Тип enum для полей сортировки.</typeparam>
        /// <returns>Объект Sorting, если парсинг выполнен успешно; в противном случае — null.</returns>
        public Sorting<TField>? ParseSortingValue<TField>() where TField : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;

            if (s.ParseFieldNameWithOperator<TField, SortOrder>(SortOrder.Asc) is not var (field, order))
                return null;

            return new Sorting<TField>(field, order);
        }
    }

    extension<T>(IQueryable<T> source)
    {
        public IQueryable<T> ApplySortings<TField>(IEnumerable<Sorting<TField>>? sortings) where TField : Enum
        {
            if (sortings is null || !sortings.Any()) return source;
            
            IOrderedQueryable<T>? orderedQuery = null;
            foreach (var sorting in sortings)
            {
                orderedQuery = orderedQuery is null 
                    ? source.ApplyFirstSorting(sorting)
                    : orderedQuery.ApplyNotFirstSorting(sorting);
            }
            
            return orderedQuery ?? source;
        }

        private IOrderedQueryable<T> ApplyNotFirstSorting<TField>(Sorting<TField> sorting) where TField : Enum
        {
            return source.ApplySortingBase(sorting, (sorting.Order == SortOrder.Asc)
                ? nameof(Queryable.ThenBy)
                : nameof(Queryable.ThenByDescending));
        }

        public IOrderedQueryable<T> ApplyFirstSorting<TField>(Sorting<TField> sorting) where TField : Enum
        {
            return source.ApplySortingBase( sorting,
                (sorting.Order == SortOrder.Asc) ? nameof(Queryable.OrderBy) : nameof(Queryable.OrderByDescending));
        }


        private IOrderedQueryable<T> ApplySortingBase<TField>(Sorting<TField> sorting, string methodName) where TField : Enum
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, sorting.Field.ToString());
            var lambda = Expression.Lambda(property, parameter);
            
            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), property.Type},
                source.Expression,
                Expression.Quote(lambda)
            );
            
            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
