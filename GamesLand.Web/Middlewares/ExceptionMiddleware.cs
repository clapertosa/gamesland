using System.Net;
using System.Text.Json;
using GamesLand.Core.Errors;

namespace GamesLand.Web.Middlewares;

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
            await HandleExceptionAsync(context, ex, _logger);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception,
        ILogger<ExceptionMiddleware> logger)
    {
        object errors = null;

        switch (exception)
        {
            case RestException re:
            {
                logger.LogError("Rest Error");
                errors = re.Errors;
                context.Response.StatusCode = (int)re.StatusCode;
            }
                break;
            case { } ex:
            {
                logger.LogError("Server Error");
                errors = string.IsNullOrWhiteSpace(ex.Message) ? "Internal server error." : ex.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
                break;
        }

        context.Response.ContentType = "application/json";

        if (errors != null)
        {
            string serializedErrors = JsonSerializer.Serialize(errors);
            await context.Response.WriteAsync(serializedErrors);
        }
    }
}