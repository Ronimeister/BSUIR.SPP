namespace Logger
{
    public class NLogLogger : ILogger
    {
        private const string LOGGER_FILE_NAME = "log.txt";

        private readonly NLog.Logger _logger;

        public NLogLogger()
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();

            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = LOGGER_FILE_NAME };

            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logfile);

            NLog.LogManager.Configuration = config;
        }

        public void Debug(string message) => _logger.Debug(message);

        public void Error(string message) => _logger.Error(message);

        public void Fatal(string message) => _logger.Fatal(message);

        public void Info(string message) => _logger.Info(message);

        public void Warn(string message) => _logger.Warn(message);
    }
}
