namespace Loggers
{
    /// <summary>
    /// Interface that all loggers should be implement
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logger info method
        /// </summary>
        /// <param name="message">logger message</param>
        void Info(string message);

        /// <summary>
        /// Logger warn method
        /// </summary>
        /// <param name="message">logger message</param>
        void Warn(string message);

        /// <summary>
        /// Logger debug method
        /// </summary>
        /// <param name="message">logger message</param>
        void Debug(string message);

        /// <summary>
        /// Logger error message
        /// </summary>
        /// <param name="message">logger message</param>
        void Error(string message);

        /// <summary>
        /// Logger fatal method
        /// </summary>
        /// <param name="message">logger message</param>
        void Fatal(string message);
    }
}
