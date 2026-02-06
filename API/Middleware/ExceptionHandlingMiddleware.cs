using System.Net;

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
            _logger.LogError(ex, "An unhandled exception occurred.");
            context.Response.ContentType = "application/json";

            var response = new ErrorResponseDto
            {
                ErrorCode = "SERVER_ERROR",
                Message = "An unexpected error occurred. Please try again later."
            };


        }

        
    
    }
    private  int GetStatusCode(Exception ex)
    {
        // You can customize this method to return different status codes based on exception types
        return ex switch
        {
           BookingConflictException=> (int)HttpStatusCode.Conflict, //error 409 for booking conflicts

           DomainRuleViolationException => 422, // Unprocessable Entity for domain rule violations
           BookingNotFoundException => (int)HttpStatusCode.NotFound, //error 404 for not found 404
           _ => (int)HttpStatusCode.InternalServerError //error 500 for all other exceptions 500
        };
    }
}