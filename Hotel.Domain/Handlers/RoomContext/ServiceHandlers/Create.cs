using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.ServiceDTOs;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Handlers.RoomContext.ServiceHandler;

public partial class ServiceHandler : IHandler
{
  private readonly IServiceRepository  _repository;
  public ServiceHandler(IServiceRepository repository)
  => _repository = repository;

  public async Task<Response<object>> HandleCreateAsync(EditorService model)
  {
    var service = new Service(model.Name,model.Price,model.Priority,model.TimeInMinutes);  

    await _repository.CreateAsync(service);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Serviço criado.",new { service.Id });
  }
}