using System.Configuration;

namespace Loggers.Implementation
{
    /// <summary>
    /// Logger that based on NLog mech
    /// </summary>
    public class NLogLogger : ILogger
    {
        private readonly string _loggerFileName;
        private readonly NLog.Logger _logger;

        /// <summary>
        /// Standart .ctor for <see cref="NLogLogger"/>
        /// </summary>
        public NLogLogger()
        {
            _loggerFileName = ConfigurationManager.AppSettings["LogFileName"];

            _logger = NLog.LogManager.GetCurrentClassLogger();

            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = _loggerFileName };

            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logfile);

            NLog.LogManager.Configuration = config;
        }

        /// <summary>
        /// Logger debug method
        /// </summary>
        /// <param name="message">logger message</param>
        public void Debug(string message) => _logger.Debug(message);

        /// <summary>
        /// Logger error method
        /// </summary>
        /// <param name="message">logger message</param>
        public void Error(string message) => _logger.Error(message);

        /// <summary>
        /// Logger fatal method
        /// </summary>
        /// <param name="message">logger message</param>
        public void Fatal(string message) => _logger.Fatal(message);

        /// <summary>
        /// Logger info method
        /// </summary>
        /// <param name="message">logger message</param>
        public void Info(string message) => _logger.Info(message);

        /// <summary>
        /// Logger warn method
        /// </summary>
        /// <param name="message">logger message</param>
        public void Warn(string message) => _logger.Warn(message);
    }
}
