using System.Text.Json;
using RestaurantApp.Models;
using RestaurantApp.Exceptions;

namespace RestaurantApp.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred while processing request");

                httpContext.Response.ContentType = "application/json";

                var statusCode = ex switch
                {
                    ResourceNotFoundException => StatusCodes.Status404NotFound,
                    ValidationException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                httpContext.Response.StatusCode = statusCode;

                var error = new ErrorResponse
                {
                    Success = false,
                    StatusCode = statusCode,
                    Message = ex.Message,
                    Errors = ex is ValidationException ve ? ve.Errors : null,
                    Timestamp = DateTime.UtcNow,
                    TraceId = httpContext.TraceIdentifier
                };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(error, options);

                await httpContext.Response.WriteAsync(json);
            }
        }
    }
}
