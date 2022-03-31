using WeatherShape.Logging.Interfaces;

namespace WeatherShape.Logging.Factories
{
    public class LoggerFactory : IShapeLogger
    {
        private readonly IServiceProvider _serviceProvider;

        public LoggerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ILogger GetApplicationLogger()
        {
            throw new NotImplementedException();
        }

        public ILogger GetPerformanceLogger()
        {
            throw new NotImplementedException();
        }

        public ILogger GetSecurityLogger()
        {
            throw new NotImplementedException();
        }

        public ILogger GetSystemLogger()
        {
            throw new NotImplementedException();
        }
    }
}
