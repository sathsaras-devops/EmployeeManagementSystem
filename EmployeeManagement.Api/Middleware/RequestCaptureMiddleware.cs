using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class RequestCaptureMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestCaptureMiddleware> _logger;

    public RequestCaptureMiddleware(
        RequestDelegate next,
        ILogger<RequestCaptureMiddleware> logger
        )
        {
            _next = next;
            _logger = logger;
        }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        _logger.LogInformation(
            "Incoming request: {Method} {Path}{QueryString}",
            request.Method,
            request.Path,
            request.QueryString);

        // Capture request headers, excluding sensitive ones.
        foreach (var header in request.Headers)
        {
            if (header.Key.Equals("Authorization",
                    StringComparison.OrdinalIgnoreCase) ||
                header.Key.Equals("Cookie",
                    StringComparison.OrdinalIgnoreCase) ||
                header.Key.Equals("Set-Cookie",
                    StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            _logger.LogInformation(
                "Header {HeaderName}: {HeaderValue}",
                header.Key,
                header.Value.ToString());
        }

        // Capture bodt only when it is small and text-based
        if (request.ContentLength is > 0 and <= 16_384 &&
            (request.ContentType?.StartsWith(
                "application/json",
                StringComparison.OrdinalIgnoreCase) == true))
        {
            request.EnableBuffering();

            using var reader = new StreamReader(
                request.Body,
                leaveOpen: true);

            var body = await reader.ReadToEndAsync();

            request.Body.Position = 0;

            _logger.LogInformation(
                "Request body: {RequestBody}",
                body);
        }

        await _next(context);
    }
}

