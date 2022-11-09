namespace LayerArchitectureExample.Core.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Extensions.Logging;

public sealed class TimedOperationLog<T> : IDisposable
{
    private readonly ILogger<T> logger;

    private readonly LogLevel logLevel;

    private readonly string message;

    private readonly object[] args;

    private readonly Stopwatch stopwatch;

    public TimedOperationLog(ILogger<T> logger, LogLevel logLevel, string message, object[] args)
    {
        this.logger = logger;
        this.logLevel = logLevel;
        this.message = message;
        this.args = args;
        this.stopwatch = Stopwatch.StartNew();
    }

    public Exception Exception { get; set; }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        this.stopwatch.Stop();

        var messageArgs = new List<object>(this.args) { this.stopwatch.ElapsedMilliseconds };

        if (this.Exception is null)
        {
            this.logger.Log(this.logLevel, $"SUCCESS: {this.message} ({{ElapsedTime}} ms)", messageArgs.ToArray());
            return;
        }

        this.logger.LogError(this.Exception, $"FAILED: {this.message} ({{ElapsedTime}} ms) - {this.Exception.Message}", messageArgs.ToArray());
    }
}
