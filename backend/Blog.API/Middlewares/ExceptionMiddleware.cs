using System.Net;
using System.Text.Json;
using Blog.Domain.Exceptions;
using FluentValidation;

namespace Blog.API.Middlewares;

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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse();

        switch (exception)
        {
            case ValidationException validationEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = "Erro de validacao";
                response.Errors = validationEx.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
                break;

            case BusinessRuleException businessEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = businessEx.Message;
                break;

            case DomainException domainEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = domainEx.Message;
                break;

            case UnauthorizedAccessException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.Message = "Nao autorizado";
                break;

            default:
                _logger.LogError(exception, "Erro nao tratado");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = "Ocorreu um erro interno";
                break;
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public List<string>? Errors { get; set; }
}
