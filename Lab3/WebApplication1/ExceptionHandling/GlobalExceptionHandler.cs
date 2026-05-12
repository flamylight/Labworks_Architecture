using System.Net;
using BLL.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace WebApplication1.ExceptionsHandling;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
        Exception exception, CancellationToken cancellationToken)
    {
        var statusCode = exception switch
        {
            BadRequestException => HttpStatusCode.BadRequest,
            NotFoundException => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError
        };
        
        httpContext.Response.StatusCode = (int)statusCode;

        await httpContext.Response.WriteAsJsonAsync(new
        {
            error = exception.Message,
            status = (int)statusCode
        }, cancellationToken);

        return true;
    }
}