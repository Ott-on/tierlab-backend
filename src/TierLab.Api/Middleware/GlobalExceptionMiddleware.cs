using System.Net;
using System.Text.Json;
using TierLab.Application.Common.Exceptions;

namespace TierLab.Api.Middleware;

/// <summary>
/// Catches unhandled exceptions and returns standardized error responses.
/// Maps known application exceptions to appropriate HTTP status codes.
/// </summary>
public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
        var (statusCode, response) = exception switch
        {
            NotFoundException ex => (
                HttpStatusCode.NotFound,
                new ErrorResponse("Not Found", ex.Message)
            ),

            ValidationException ex => (
                HttpStatusCode.UnprocessableEntity,
                new ErrorResponse("Validation Failed", ex.Message, ex.Errors)
            ),

            BusinessRuleException ex => (
                HttpStatusCode.Conflict,
                new ErrorResponse("Business Rule Violation", ex.Message)
            ),

            UnauthorizedAccessException => (
                HttpStatusCode.Forbidden,
                new ErrorResponse("Forbidden", "You do not have permission to perform this action.")
            ),

            _ => (
                HttpStatusCode.InternalServerError,
                new ErrorResponse("Internal Server Error", "An unexpected error occurred.")
            )
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
        }
        else
        {
            _logger.LogWarning("Handled exception ({StatusCode}): {Message}", (int)statusCode, exception.Message);
        }

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        await context.Response.WriteAsync(json);
    }
}

internal sealed record ErrorResponse(
    string Title,
    string Detail,
    IReadOnlyDictionary<string, string[]>? Errors = null
);
