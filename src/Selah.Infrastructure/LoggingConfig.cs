using System;
using Microsoft.Extensions.Logging;

namespace Selah.Infrastructure
{
  public class LoggingConfig: ILoggerProvider
  {
    public ILogger CreateLogger(string categoryName)
      {
        return new SelahLogger(categoryName);
      }

      public void Dispose()
      {
      }

      public class SelahLogger : ILogger
      {
        private readonly string _categoryName;

        public SelahLogger(string categoryName)
        {
          _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
          return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
          return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
          Func<TState, Exception, string> formatter)
        {
          if (!IsEnabled(logLevel)) return;

          Console.WriteLine(
            $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} selah-api: {getShortLogLevel(logLevel).ToUpper()} {_categoryName}  - {(eventId.Id == 0 ? "" : $"Event ID: {eventId.Id.ToString()}:")} {formatter(state, exception)}");
        }

        private string getShortLogLevel(LogLevel level)
        {
          switch (level)
          {
            case LogLevel.Trace:
              return "TRACE";
            case LogLevel.Debug:
              return "DEBUG";
            case LogLevel.Information:
              return "INFO";
            case LogLevel.Warning:
              return "WARN";
            case LogLevel.Error:
              return "ERROR";
            case LogLevel.Critical:
              return "CRIT";
            default:
              throw new ArgumentOutOfRangeException(nameof(level));
          }
        }
      }
  }
}