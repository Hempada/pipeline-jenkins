using BlazorBootstrap;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace WebUi.Extensions
{
    public static class ODataExtensions
    {
        public static string ODataQuery<T>(this GridDataProviderRequest<T> request)
        {
            string sortString = "";
            string sortDirectionString = "";

            if (request.Sorting is not null && request.Sorting.Any())
            {
                sortString = request.Sorting.FirstOrDefault()!.SortString;
                var sortDirection = request.Sorting.FirstOrDefault()!.SortDirection;
                sortDirectionString = sortDirection == SortDirection.Ascending ? "asc" : "desc";
            }

            string orderBy = !string.IsNullOrEmpty(sortString) ? $"&$orderby={sortString} {sortDirectionString}" : string.Empty;
            string pagination = $"&$top={request.PageSize}&$skip={request.PageSize * (request.PageNumber - 1)}";

            StringBuilder filters = new StringBuilder();
            foreach (var filter in request.Filters)
            {
                filters.Append(BuildODataQuery<T>(filter));
            }

            return orderBy + pagination + filters.ToString();
        }

        private static string BuildODataQuery<T>(FilterItem filter)
        {
            string propertyName = filter.PropertyName;
            string type = "";

            if (propertyName.Contains(" as "))
            {
                var split = propertyName.Split(" as ");
                propertyName = split[0];
                type = split[1].ToLower();
            }
            else
            {
                type = typeof(T).GetPropertyType(filter.PropertyName)?.ToString()
                    .Replace("System.", "").ToLower() ?? string.Empty;
            }

            var value = type == "string" || type == "datetime" ? $"'{filter.Value}'" : filter.Value;

            string? filterOp = null;

            switch (filter.Operator)
            {
                case FilterOperator.Contains:
                    return $"&$filter=contains({propertyName}, {value})";
                case FilterOperator.DoesNotContain:
                    return $"&$filter=not(contains({propertyName}, {value}))";
                case FilterOperator.StartsWith:
                    return $"&$filter=startswith({propertyName}, {value})";
                case FilterOperator.EndsWith:
                    return $"&$filter=endswith({propertyName}, {value})";
                case FilterOperator.IsNull:
                    return $"&$filter={propertyName} eq null";
                case FilterOperator.IsNotNull:
                    return $"&$filter={propertyName} ne null";
                case FilterOperator.IsEmpty:
                    return $"&$filter={propertyName} eq ''";
                case FilterOperator.IsNotEmpty:
                    return $"&$filter={propertyName} ne ''";

                case FilterOperator.Equals:
                    filterOp = "eq";
                    break;
                case FilterOperator.NotEquals:
                    filterOp = "ne";
                    break;
                case FilterOperator.GreaterThan:
                    filterOp = "gt";
                    break;
                case FilterOperator.LessThan:
                    filterOp = "lt";
                    break;
                case FilterOperator.GreaterThanOrEquals:
                    filterOp = "ge";
                    break;
                case FilterOperator.LessThanOrEquals:
                    filterOp = "le";
                    break;
            }

            if (filterOp is not null)
            {
                return $"&$filter={propertyName} {filterOp} {value}";
            }

            return string.Empty;
        }
    }

    public sealed record ODataCountValue<T>
    {
        [JsonPropertyName("@odata.count")]
        public int Count { get; init; }

        [JsonPropertyName("value")]
        public IEnumerable<T> Value { get; init; }

        public ODataCountValue()
        {
            Value = Enumerable.Empty<T>();
        }
    }
}
