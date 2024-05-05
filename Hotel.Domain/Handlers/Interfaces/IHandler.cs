
namespace Hotel.Domain.Handlers.Interfaces;

public interface IHandler<TRequest, TResponse>
{
  Task<TResponse> HandleAsync(TRequest request);
}