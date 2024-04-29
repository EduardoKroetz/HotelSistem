using Hotel.Domain.Middlewares;

namespace Hotel.Domain.Extensions;

public static class HandleExceptionsMiddlewareExtensions
{
  public static IApplicationBuilder UseHandleExceptions(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<HandleExceptionMiddleware>();
  }
}