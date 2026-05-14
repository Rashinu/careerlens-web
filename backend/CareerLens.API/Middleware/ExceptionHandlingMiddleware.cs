using CareerLens.Application.Common.Exceptions;
using CareerLens.Shared.Wrappers;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace CareerLens.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        var (statusCode, message) = exception switch
        {
            ValidationException ve => (HttpStatusCode.BadRequest, string.Join("; ", ve.Errors.Select(e => e.ErrorMessage))),
            NotFoundException nfe => (HttpStatusCode.NotFound, nfe.Message),
            UnauthorizedException ue => (HttpStatusCode.Unauthorized, ue.Message),
            InvalidOperationException ioe => (HttpStatusCode.Conflict, ioe.Message),
            _ => (HttpStatusCode.InternalServerError, "Beklenmeyen bir hata oluştu.")
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Beklenmeyen hata: {Message}", exception.Message);

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = ApiResponse<object>.Fail(message);
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }
}
