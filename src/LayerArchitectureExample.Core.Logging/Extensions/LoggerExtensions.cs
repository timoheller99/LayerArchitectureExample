namespace LayerArchitectureExample.Core.Logging.Extensions;

using Microsoft.Extensions.Logging;

public static class LoggerExtensions
{
    public static TimedOperationLog<T> TimedOperation<T>(this ILogger<T> logger, LogLevel logLevel, string message, params object[] args)
    {
        return new TimedOperationLog<T>(logger, logLevel, message, args);
    }
}
