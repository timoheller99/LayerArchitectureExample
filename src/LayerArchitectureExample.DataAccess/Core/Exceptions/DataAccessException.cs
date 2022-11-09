namespace LayerArchitectureExample.DataAccess.Core.Exceptions;

using System;

/// <inheritdoc />
public class DataAccessException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataAccessException"/> class.
    /// </summary>
    public DataAccessException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataAccessException"/> class.
    /// </summary>
    public DataAccessException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataAccessException"/> class.
    /// </summary>
    public DataAccessException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
