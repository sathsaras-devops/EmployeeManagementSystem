using System.Net;
using System.Text.Json;
using FluentValidation;
using EmployeeManagement.Application.Exceptions;

namespace EmployeeManagement.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
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
        catch (ValidationException ex)
        {
            _logger.LogWarning(
                ex,
                "Validation failed.");

            await WriteResponseAsync(
                context,
                HttpStatusCode.BadRequest,
                new
                {
                    message = "Validation failed.",
                    errors = ex.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                            x => x.Key,
                            x => x.Select(y => y.ErrorMessage).ToArray())
                });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(
                ex,
                "Resource not found.");

            await WriteResponseAsync(
                context,
                HttpStatusCode.NotFound,
                new
                {
                    message = ex.Message
                });
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(
                ex,
                "Business rule violation.");

            await WriteResponseAsync(
                context,
                HttpStatusCode.Conflict,
                new
                {
                    message = ex.Message
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception.");

            await WriteResponseAsync(
                context,
                HttpStatusCode.InternalServerError,
                new
                {
                    message = "An unexpected error occurred."
                });
        }
    }

    private static async Task WriteResponseAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        object response)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}