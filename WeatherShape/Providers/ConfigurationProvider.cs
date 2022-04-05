

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using WeatherShape.Configuration.Models;

namespace WeatherShape
{
    public static class ConfigurationProvider
    {

        private static IConfiguration Configuration { get; set; }

        private static bool _shutDownOnError;
        private static Serilog.ILogger _logger;

        /// <summary>
        /// Initializes the configuration provider with configuration and logger
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="shutDownOnError"></param>
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

        /// <summary>
        /// Adds and validates the configuration, if a given configuration is not validated
        /// the application will shutdown.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        public static void AddAndValidateConfiguration<T>(this IServiceCollection services) where T : Validatable
        {
            var configruationName = typeof(T).Name;

            var validatableConfiguration = HandleConfigurationRetrieval<T>();
                
            services.AddOptions<T>()
                    .Bind(Configuration.GetSection(configruationName)).Validate(HandleConfigurationValidation);
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
