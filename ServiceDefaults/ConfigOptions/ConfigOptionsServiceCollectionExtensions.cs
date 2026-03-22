using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ServiceDefaults.ConfigOptions;

public static class ConfigOptionsServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public OptionsBuilder<TOptions> AddConfigOptions<TOptions>() where TOptions : class, IConfigOptions
        {
            return services.AddOptions<TOptions>()
                .BindConfiguration(TOptions.SectionName)
                .ValidateOnStart();
        }

        public OptionsBuilder<TOptions> AddConfigOptions<TOptions, TValidator>() where TOptions : class, IConfigOptions
            where TValidator : class, IValidateOptions<TOptions>
        {
            var optionsBuilder = services.AddConfigOptions<TOptions>();
            services.AddSingleton<IValidateOptions<TOptions>, TValidator>();
            return optionsBuilder;
        }

        public OptionsBuilder<TOptions> AddConfigOptions<TOptions>(Func<TOptions, bool> validation)
            where TOptions : class, IConfigOptions
        {
            return services.AddConfigOptions<TOptions>().Validate(validation);
        }

        public OptionsBuilder<TOptions> AddConfigOptions<TOptions, TDep>(Func<TOptions, TDep, bool> validation)
            where TOptions : class, IConfigOptions where TDep : notnull
        {
            return services.AddConfigOptions<TOptions>().Validate(validation);
        }
    }
}