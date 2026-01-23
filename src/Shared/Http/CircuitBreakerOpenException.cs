namespace Shared.Http;

public sealed class CircuitBreakerOpenException : InvalidOperationException
{
    public CircuitBreakerOpenException(string message) : base(message)
    {
    }
}
