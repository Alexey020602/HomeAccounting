using MudBlazor;

namespace BlazorConsolidated.Common;

public static class MudGridStateExtensions
{
    extension<T>(GridState<T> gridState)
    {
        public string? FilterQuery
        {
            get
            {
                var filterValues = gridState.FilterDefinitions.Select(f => f.QueryValue)
                    .Where(f => !string.IsNullOrWhiteSpace(f));

                if(!filterValues.Any()) return null;
                
                return string.Join(',', filterValues);
            }
        }

        public string? SortingQuery
        {
            get
            {
                var sortingValues = gridState.SortDefinitions
                    .Select(s=>s.QueryValue)
                    .Where(f => !string.IsNullOrWhiteSpace(f));
                
                if(!sortingValues.Any()) return null;
                
                return string.Join(',', sortingValues);
            }
        }
    }

    extension<T>(SortDefinition<T> sortDefinition)
    {
        public string QueryValue
        {
            get
            {
                if(string.IsNullOrWhiteSpace(sortDefinition.SortBy)) return "";
                var queryOperator = sortDefinition.Descending ? "desc" : "asc";
                
                return $"{sortDefinition.SortBy}[{queryOperator}]";
            }
        }
    }

    extension<T>(IFilterDefinition<T> filter)
    {
        public string QueryValue
        {
            get
            {
                var fieldName = filter.FieldName;
                if (string.IsNullOrWhiteSpace(fieldName)) return "";
                
                var operatorBlock = filter.QueryOperator is var filterOperator ? $"[{filterOperator}]" : "";
                var value = filter.FilterValue ?? "";
                return $"{fieldName}{operatorBlock}={value}";
            }
        }

        public string? FieldName
        {
            get
            {
                if (filter.Column is not { PropertyName: var propertyName} || propertyName is null ) return null;
                var dotIndex = propertyName.IndexOf('.');
                return dotIndex == -1 ? propertyName : propertyName[..dotIndex];
            }
        }

        private string? QueryOperator => filter.Operator switch
        {
            "=" => "eq",
            "!=" => "ne",
            ">" => "gt",
            "<" => "lt",
            ">=" => "gte",
            "<=" => "lte",
            "is" => "eq",
            "is not" => "ne",
            "is after" => "gt",
            "is on or after" => "gte",
            "is before" => "lt",
            "is on or before" => "lte",
            "equal" => "eq",
            "not equal" => "ne",
            "contains" => "cn",
            "not contains" => "ne",
            "starts with" => "sw",
            "ends with" => "ew",
            _ => null,
        };

        private string? FilterValue => filter.Value?.ToString();
    }
}