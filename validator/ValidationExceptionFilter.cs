using System.ComponentModel.DataAnnotations;

public class ValidationExceptionFilter : IEndpointFilter
{
    private readonly ILogger<ValidationExceptionFilter> _logger;

    public ValidationExceptionFilter(ILogger<ValidationExceptionFilter> logger)
    {
        _logger = logger;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            // Call the next middleware or endpoint
            var result = await next(context);// (context.HttpContext);

            // If the result is successful, return it
            return result;
        }
        catch (ValidationException ex)
        {
            // Log and cache the validation errors
            _logger.LogError(ex, "Validation failed");

            // Build a BadRequest response with validation errors
            var errorResponse = new
            {
                Errors = ex.Message
            };

            return Results.BadRequest(errorResponse);
        }
    }

}
