namespace HomeAccounting.Common.Application.Paging;

public static class FilterExtensions
{
    extension(FilterOperator)
    {
        // public static bool TryParse(string s, out FilterOperator filterOperator)
        // {
        //     switch (s.ToLowerInvariant())
        //     {
        //         case "eq": filterOperator = FilterOperator.Equal; return true;
        //         case "ne": filterOperator = FilterOperator.NotEqual; return true;
        //         case "gt": filterOperator = FilterOperator.GreaterThan; return true;
        //         case  "gte": filterOperator = FilterOperator.GreaterThanOrEqual; return true;
        //         case "lt": filterOperator = FilterOperator.LessThan; return true;
        //         case "lte": filterOperator = FilterOperator.LessThanOrEqual; return true;
        //         // case "in": filterOperator = FilterOperator.In; return true;
        //         // case "notin": filterOperator = FilterOperator.NotIn; return true;
        //         default: filterOperator = FilterOperator.Equal;
        //             return false;
        //     }
        // }

        public static FilterOperator? Parse(string s) => s.ToLowerInvariant() switch
        {
            "eq" => FilterOperator.Equal,
            "ne" => FilterOperator.NotEqual,
            "gt" => FilterOperator.GreaterThan,
            "gte" => FilterOperator.GreaterThanOrEqual,
            "lt" => FilterOperator.LessThan,
            "lte" => FilterOperator.LessThanOrEqual,
            // "in" => FilterOperator.In,
            // "notin" => FilterOperator.NotIn,
            _ => null//FilterOperator.Equal
        };
    }

    extension(string s) 
    {
        public IEnumerable<Filter<TField>>? ParseFilter<TField>() where TField : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            
            return [
                .. s.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(x => x.ParseFilterValue<TField>())
                    .Where(f => f is not null)
                    .Cast<Filter<TField>>()
            ];
        }
        public Filter<TField>? ParseFilterValue<TField>() where TField: struct, Enum
        {
            var equalsIndex = s.IndexOf('=');
            if (equalsIndex < 0)
                return null;
            
            var left = s[..equalsIndex].Trim();
            var value = s[(equalsIndex + 1)..].Trim();
            
            if (string.IsNullOrWhiteSpace(value))
                return null;

            // var leftParts = left.Split("[");
            
            if (left.ParseFieldNameWithSquareBrackets() is not var (fieldName, operatorName))
                return null;
            
            if (!Enum.TryParse<TField>(fieldName, true, out var field))
                return null;

            if (operatorName is null) 
                return new Filter<TField>(field, FilterOperator.Equal, value);
            
            if (FilterOperator.Parse(operatorName) is not {} op)
                return null;
            
            return new Filter<TField>(field, op, value);
            
            
            // if (leftParts is {  Length: 0 or > 2 }) 
            //     return null;
            // var fieldName = leftParts[0].Trim();
            //
            // if (string.IsNullOrWhiteSpace(fieldName))
            //     return null;
            //
            // if (!Enum.TryParse<TField>(fieldName, true, out var field)) 
            //     return null;
            //
            // var op = FilterOperator.Equal;
            // if (leftParts is not { Length: 2 }) return new Filter<TField>(field, op, value);
            //
            // var operatorString = leftParts[1].Trim();
            //     
            // if(!operatorString.EndsWith("]"))
            //     return null;
            //     
            // op = FilterOperator.Parse(operatorString.TrimEnd(']'));

            // return new Filter<TField>(field, op, value);
        }
        
        public (string, string?)? ParseFieldNameWithSquareBrackets()
        {
            var parts =  s.Split('[');

           if(parts is  { Length: <= 0 or > 2 })
               return null;
           
           var fieldName = parts[0].Trim();
           
           if  (string.IsNullOrWhiteSpace(fieldName))
               return null;
           
           if (parts is { Length: 1}) 
               return (fieldName, null);
           
           var operatorString = parts[1];
           
           if(!operatorString.EndsWith(']'))
               return null;
           
           var operatorName = operatorString.TrimEnd(']').Trim();

           if (string.IsNullOrWhiteSpace(operatorName))
               return null;
           
           return (fieldName, operatorName);
        }
    }

    
    
}