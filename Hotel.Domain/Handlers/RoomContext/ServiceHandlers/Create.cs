using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.ServiceDTOs;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces.EmployeeContext;
using Hotel.Domain.Repositories.Interfaces.RoomContext;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Handlers.RoomContext.ServiceHandler;

public partial class ServiceHandler : IHandler
{
  private readonly IServiceRepository  _repository;
  private readonly IResponsabilityRepository _responsabilityRepository;
  public ServiceHandler(IServiceRepository repository, IResponsabilityRepository responsabilityRepository)
  {
    _repository = repository;
    _responsabilityRepository = responsabilityRepository;
  }


  public async Task<Response> HandleCreateAsync(EditorService model)
  {
    var service = new Service(model.Name,model.Price,model.Priority,model.TimeInMinutes);  

    try
    {
      await _repository.CreateAsync(service);
      await _repository.SaveChangesAsync();
    }
    catch (DbUpdateException e)
    {
      if (e.InnerException != null && e.InnerException.ToString().Contains(model.Name))
        return new Response(400, "Esse nome já está cadastrado.");
      else
        return new Response(500, "Algum erro ocorreu ao salvar no banco de dados.");
    }

    return new Response(200,"Serviço criado com sucesso!",new { service.Id });
  }
}