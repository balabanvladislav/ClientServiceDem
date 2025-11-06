using System.Security;
using System.Text.Json;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Host.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            // Domain/Business Logic Exceptions
            IEntityNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            
            IDuplicateEntityException => (StatusCodes.Status409Conflict, exception.Message),
        
            UserNotFoundException => (StatusCodes.Status401Unauthorized, exception.Message),
            
            // Validation Exceptions
            ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
        
            // Security Exceptions
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
            SecurityException => (StatusCodes.Status403Forbidden, "Access forbidden"),
        
            // Operation Exceptions
            InvalidOperationException => (StatusCodes.Status409Conflict, exception.Message),
            NotSupportedException => (StatusCodes.Status501NotImplemented, "Operation not supported"),
        
            // Timeout/Cancellation
            OperationCanceledException => (StatusCodes.Status408RequestTimeout, "Request was cancelled or timed out"),
            TimeoutException => (StatusCodes.Status408RequestTimeout, "Request timed out"),
        
            // Database/External Service Exceptions (be careful about exposing internal details)
            DbUpdateException => (StatusCodes.Status409Conflict, "A conflict occurred while saving data"),
            HttpRequestException => (StatusCodes.Status502BadGateway, "External service unavailable"),
            NullReferenceException => (StatusCodes.Status500InternalServerError, "A null reference occurred"),
        
            // Catch-all
            _ => (StatusCodes.Status500InternalServerError, "An error occurred while processing your request")
        };


        context.Response.StatusCode = statusCode;

        var response = new
        {
            error = message,
            statusCode
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}