namespace HomeAccounting.Common.Application.Paging;

public static class SortingExtensions
{
    extension(string s)
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
}
