namespace Adf.Core
{
    #region usings

    using Microsoft.Extensions.Logging;

    #endregion

    public class ApplicationLogging
    {
        /// <summary>
        /// The logger factory.
        /// </summary>
        private static ILoggerFactory _loggerFactory;

        public static void ConfigureLogger(ILoggerFactory factory)
        {
            _loggerFactory = factory;
        }

        public static ILogger CreateLogger<T>()
        {
            return _loggerFactory.CreateLogger<T>();
        }
    }
}
