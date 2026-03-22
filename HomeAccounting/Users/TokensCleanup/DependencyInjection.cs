using ServiceDefaults.ConfigOptions;

namespace HomeAccounting.Users.TokensCleanup;

internal static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public void AddTokensCleanup()
        {
            services.AddConfigOptions<TokensCleanupOptions>(opt=> opt.DayInterval > 0);
            services.AddHostedService<TokensCleanupWorker>();
        }
    }
}