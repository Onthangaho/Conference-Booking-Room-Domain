using System.Net;
using ConferenceBookingRoomAPI;
using ConferenceBookingRoomDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;


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

            var (statusCode, category) = MapException(ex);
            _logger.LogError(ex, "Exception caught at {Time}. Path: {Path}, Method: {Method}"
            , DateTime.UtcNow,
              context.Request.Path,
              context.Request.Method);

              context.Response.ContentType = "application/json";
              context.Response.StatusCode = statusCode;

              var errorResponse = new ErrorResponseDto
              {
                  ErrorCode = ex.GetType().Name.ToUpperInvariant(),
                  Message = statusCode>=500 ? "An unexpected error occurred. Please try again later." : ex.Message,
                  Category = category
              };
              await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }



    }
    private (int statusCode, string category) MapException( Exception ex)
    {
        // You can customize this method to return different status codes based on exception types
        return ex switch
        {
          BookingConflictException => ((int)HttpStatusCode.Conflict, "BusinessRuleViolation"),
          DomainRuleViolationException=>(422, "BusinessRuleViolation"),
            ConferenceRoomNotFoundException => ((int)HttpStatusCode.NotFound, "ClientError"),
            BookingNotFoundException => ((int)HttpStatusCode.NotFound, "ClientError"),
            BookingDeleteConflictException => ((int)HttpStatusCode.Conflict, "BusinessRuleViolation"),
            InvalidBookingTimeException => (422, "BusinessRuleViolation"),
            InfrastructureFailureException => ((int)HttpStatusCode.InternalServerError, "InfrastructureFailure"),
            _ => ((int)HttpStatusCode.InternalServerError, "UnexpectedError")
        };
    }
}