using System.Net;
using System.Text.Json;
using Newton.GameStore.Domain.Exceptions;

namespace Newton.GameStore.API.Middleware;

/// <summary>
/// Middleware for handling exceptions globally and returning appropriate HTTP responses.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ErrorResponse();

        switch (exception)
        {
            case NotFoundException notFoundEx:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Message = notFoundEx.Message;
                _logger.LogWarning(notFoundEx, "Resource not found");
                break;

            case DomainValidationException validationEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = validationEx.Message;
                _logger.LogWarning(validationEx, "Validation error");
                break;

            case DomainException domainEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = domainEx.Message;
                _logger.LogWarning(domainEx, "Domain error");
                break;

            case ArgumentNullException argNullEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = argNullEx.Message;
                _logger.LogWarning(argNullEx, "Argument null error");
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = "An unexpected error occurred.";
                _logger.LogError(exception, "Unhandled exception occurred");
                break;
        }

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(errorResponse, options);
        await response.WriteAsync(json);
    }
}

/// <summary>
/// Standard error response format.
/// </summary>
public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Extension methods for registering the exception handling middleware.
/// </summary>
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
