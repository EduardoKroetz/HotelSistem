using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Net;

namespace Hotel.Domain.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var response = new Response([exception.Message]);

        switch (exception)
        {
            case ValidationException e:
            case ArgumentException:
            case InvalidOperationException:
            case StripeException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            break;
            case NotFoundException e:
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            break;
            case UnauthorizedAccessException e:
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            break;
            case DbUpdateException _:
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response = new Response([$"Não foi possível atualizar no banco de dados."]);
            break;
            default:
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            break;
        }


        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}