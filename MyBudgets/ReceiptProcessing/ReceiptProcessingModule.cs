using MyBudgets.ReceiptProcessing.Contracts;
using MyBudgets.ReceiptProcessing.GetReceiptData;
using Refit;

namespace MyBudgets.ReceiptProcessing;

internal static class ReceiptProcessingModule
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddReceiptProcessingModule()
        {
            services.AddReceiptData();
            services.AddTransient<IReceiptProcessService, ReceiptProcessService>();
            return services;
        }

        private IServiceCollection AddReceiptData()
        {
            services.AddRefitClient<ICheckService>()
                .ConfigureHttpClient(c => { c.BaseAddress = new Uri("https://proverkacheka.com"); });
            
            return services;
        }
    }
}