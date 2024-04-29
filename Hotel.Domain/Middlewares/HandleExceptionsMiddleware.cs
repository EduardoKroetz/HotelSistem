using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Middlewares;

public class HandleExceptionMiddleware 
{
  private readonly RequestDelegate _next;
  public HandleExceptionMiddleware(RequestDelegate next)
  =>  _next = next;
  
  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch(ValidationException)
    {

    }
  }

}