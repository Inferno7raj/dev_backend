// ErrorHandlingMiddleware.cs
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using PersonalDetailsAPI.Exceptions;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
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

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var response = new ApiResponse<string>
        {
            Success = false,
            Message = "An error occurred while processing your request.",
            Errors = new List<string> { ex.Message }
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // Default to 500

        if (ex is DuplicateEntryException) // Example for a custom exception
        {
            response.Success = false;
            response.Message = "Duplicate entry error.";
            response.Errors.Add(ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }

        _logger.LogError(ex, "An unhandled exception occurred during request processing.");

        return context.Response.WriteAsJsonAsync(response);
    }
}
