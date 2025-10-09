using Microsoft.Extensions.DependencyInjection;
using Shared.Utils.BarCode;

namespace Receipts.UI;

public static class DependencyInjection
{
    public static IServiceCollection AddReceipt(this IServiceCollection services) => services.AddBarcode();
}