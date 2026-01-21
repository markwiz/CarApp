using System;

namespace Car.Core.Exceptions;

public sealed class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
    }
}
