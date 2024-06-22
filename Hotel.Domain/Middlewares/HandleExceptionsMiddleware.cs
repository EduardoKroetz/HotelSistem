using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Net;

namespace Hotel.Domain.Middlewares;

public class HandleExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public HandleExceptionMiddleware(RequestDelegate next)
    => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(
              new Response([e.Message])
            );
        }
        catch (ArgumentException e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(
              new Response([e.Message])
            );
        }
        catch (NotFoundException e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsJsonAsync(
              new Response([e.Message])
            );
        }
        catch (InvalidOperationException e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(
              new Response([e.Message])
            );
        }
        catch (UnauthorizedAccessException e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            await context.Response.WriteAsJsonAsync(
              new Response ([e.Message])
            );
        }
        catch (StripeException e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(
              new Response([$"Ocorreu uma falha ao lidar com o serviço do Stripe. Mensagem de erro: {e.Message}"])
            );
        }
        catch (DbUpdateException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(
              new Response([$"Não foi possível atualizar no banco de dados."])
            );
        }
        catch (Exception e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Console.WriteLine(e.Message);
            await context.Response.WriteAsJsonAsync(
              new Response([e.Message])
            );
        }
    }

}