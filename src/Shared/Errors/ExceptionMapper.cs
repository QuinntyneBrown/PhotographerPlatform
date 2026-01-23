namespace Shared.Errors;

public static class ExceptionMapper
{
    private static readonly Dictionary<Type, Func<Exception, ProblemDetails>> Mappings = new()
    {
        {
            typeof(ArgumentException),
            ex => new ProblemDetails
            {
                Title = "Invalid request.",
                Status = 400,
                Detail = ex.Message
            }
        },
        {
            typeof(ArgumentNullException),
            ex => new ProblemDetails
            {
                Title = "Missing required value.",
                Status = 400,
                Detail = ex.Message
            }
        },
        {
            typeof(UnauthorizedAccessException),
            ex => new ProblemDetails
            {
                Title = "Unauthorized.",
                Status = 401,
                Detail = ex.Message
            }
        },
        {
            typeof(InvalidOperationException),
            ex => new ProblemDetails
            {
                Title = "Invalid operation.",
                Status = 409,
                Detail = ex.Message
            }
        }
    };

    public static ProblemDetails ToProblemDetails(Exception exception, string? traceId = null)
    {
        if (exception is null)
        {
            return new ProblemDetails();
        }

        var type = exception.GetType();
        foreach (var mapping in Mappings)
        {
            if (mapping.Key.IsAssignableFrom(type))
            {
                var details = mapping.Value(exception);
                if (!string.IsNullOrWhiteSpace(traceId))
                {
                    details.Extensions["traceId"] = traceId;
                }

                return details;
            }
        }

        var fallback = new ProblemDetails
        {
            Title = "Unexpected error.",
            Status = 500,
            Detail = exception.Message
        };

        if (!string.IsNullOrWhiteSpace(traceId))
        {
            fallback.Extensions["traceId"] = traceId;
        }

        return fallback;
    }

    public static void Register<TException>(Func<TException, ProblemDetails> mapper)
        where TException : Exception
    {
        Mappings[typeof(TException)] = ex => mapper((TException)ex);
    }
}
