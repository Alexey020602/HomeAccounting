namespace HomeAccounting.Common.Application.Paging;

public static class StringExtensions
{
    extension(string s)
    {
        public (TField, TOperator)? ParseFieldNameWithOperator<TField, TOperator>(TOperator defaultValue = default)
            where TField : struct, Enum where TOperator : struct, Enum
        {
            var parts = s.Split('[');
            if (parts is { Length: <= 0 or > 2 })
                return null;
            var fieldName = parts[0].Trim();
            
            if (!Enum.TryParse<TField>(fieldName, true, out var field))
                return null;
            
            if (parts is { Length: 1 })
                return (field, defaultValue);
            var operatorString = parts[1];

            if (!operatorString.EndsWith(']'))
                return null;

            var operatorName = operatorString.TrimEnd(']').Trim();
            
            if (!Enum.TryParse<TOperator>(operatorName, true, out var operatorValue))
                return null;
            
            return (field, operatorValue);
        }
        
        
        /// <summary>
        /// Парсит строку с именем поля и опциональным оператором в квадратных скобках.
        /// </summary>
        /// <returns>Кортеж с именем поля и оператором (если указан), если парсинг выполнен успешно; в противном случае — null.</returns>
        public (string, string?)? ParseFieldNameWithSquareBrackets()
        {
            var parts = s.Split('[');

            if (parts is { Length: <= 0 or > 2 })
                return null;

            var fieldName = parts[0].Trim();

            if (string.IsNullOrWhiteSpace(fieldName))
                return null;

            if (parts is { Length: 1 })
                return (fieldName, null);

            var operatorString = parts[1];

            if (!operatorString.EndsWith(']'))
                return null;

            var operatorName = operatorString.TrimEnd(']').Trim();

            if (string.IsNullOrWhiteSpace(operatorName))
                return null;

            return (fieldName, operatorName);
        }
    }
}