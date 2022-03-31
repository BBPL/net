

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using WeatherShape.Configuration.Models;

namespace Shape.Weather.Common
{
    public static class ConfigurationProvider
    {

        private static IConfiguration Configuration { get; set; }

        private static bool _shutDownOnError;
        private static ILogger _logger;

        public static void InitializeConfigurationProvider(
            this IServiceCollection services,
            IConfiguration configuration,
            bool shutDownOnError = false)
        {
            Configuration = configuration;
            _shutDownOnError = shutDownOnError;
            _logger = services.BuildServiceProvider()
                              .GetRequiredService<Serilog.ILogger>();
        }

        public static void AddAndValidateConfiguration<T>(this IServiceCollection services) where T : Validatable
        {
            var configruationName = typeof(T).Name;

            var validatableConfiguration = HandleConfigurationRetrieval<T>();
            if (validatableConfiguration != null)
            {
                services.AddSingleton<T>(validatableConfiguration);
            }
            else
            {
                throw new InvalidOperationException();
            }

            services.AddOptions<T>()  
                .Bind(validatableConfiguration).Validate(HandleConfigurationValidation);
        }

        private static bool HandleConfigurationValidation(Validatable configuration)
        {
            var result = configuration.Validate(_logger);

            if (!result && _shutDownOnError)
            {
                Environment.Exit(1);
            }

            return result;
        }

        private static T HandleConfigurationRetrieval<T>() where T : Validatable
        {
            T? configuration = default;
            var configurationName = typeof(T).Name;

            try
            {
                configuration = Configuration.GetSection(configurationName).Get<T>();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, ex.Message);
                if (_shutDownOnError)
                {
                    Environment.Exit(1);
                }
                else
                {
                    throw;
                }
            }

            return configuration;
        }
    }
}
