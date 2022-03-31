namespace WeatherShape.Logging.Interfaces
{
    public interface IShapeLogger
    {
        /// <summary>
        ///     Produces application logger. If this is first call in scope it will construct new logger.
        ///     If it's consecutive call or getNew parameter is true it will create new Logger.
        /// </summary>
        /// <returns></returns>
        ILogger GetApplicationLogger();

        /// <summary>
        ///     Produces security logger. If this is first call in scope it will construct new logger.
        ///     If it's consecutive call or getNew parameter is true it will create new Logger.
        /// </summary>
        /// <returns></returns>
        ILogger GetSecurityLogger();

        /// <summary>
        ///     Produces system logger. If this is first call in scope it will construct new logger.
        ///     If it's consecutive call or getNew parameter is true it will create new Logger.
        /// </summary>
        /// <returns></returns>
        ILogger GetSystemLogger();

        /// <summary>
        ///     Produces performance logger. If this is first call in scope it will construct new logger.
        ///     If it's consecutive call or getNew parameter is true it will create new Logger.
        /// </summary>
        /// <returns></returns>
        ILogger GetPerformanceLogger();
    }
}
