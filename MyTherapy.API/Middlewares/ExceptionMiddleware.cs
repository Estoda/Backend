using System.Net;
using System.Text.Json;

namespace MyTherapy.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = ex switch
        {
            KeyNotFoundException => (int)HttpStatusCode.NotFound, // 404
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, // 401
            ArgumentException => (int)HttpStatusCode.BadRequest, // 400
            _ => (int)HttpStatusCode.InternalServerError // 500
        };

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = ex.Message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
