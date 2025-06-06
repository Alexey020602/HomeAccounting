using Shared.Model;
using MudDateRange = MudBlazor.DateRange;
namespace BlazorShared.Extensions;

public static class DateRangeExtensions
{
    public static DateRange ToDateRange(this MudDateRange dateRange) =>
        new()
        {
            Start = dateRange.Start,
            End = dateRange.End
        };

    public static MudDateRange ToMudDateRange(this DateRange dateRange) => new MudDateRange()
    {
        Start = dateRange.Start,
        End = dateRange.End
    };
}