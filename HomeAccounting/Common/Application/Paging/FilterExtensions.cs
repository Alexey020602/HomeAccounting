namespace HomeAccounting.Common.Application.Paging;

public static class FilterExtensions
{
    extension(string s)
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
}