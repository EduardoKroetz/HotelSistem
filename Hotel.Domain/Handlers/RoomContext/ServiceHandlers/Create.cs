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
  private readonly IResponsibilityRepository _responsibilityRepository;
  public ServiceHandler(IServiceRepository repository, IResponsibilityRepository responsibilityRepository)
  {
    _repository = repository;
    _responsibilityRepository = responsibilityRepository;
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
      if (e.InnerException != null && e.InnerException.ToString().Contains("Name"))
        throw new ArgumentException("Esse nome já está cadastrado.");
      else
        throw new Exception("Algum erro ocorreu ao salvar no banco de dados.");
    }

    return new Response(200,"Serviço criado com sucesso!",new { service.Id });
  }
}