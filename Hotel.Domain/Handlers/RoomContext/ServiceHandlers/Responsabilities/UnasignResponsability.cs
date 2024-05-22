using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.RoomContext.ServiceHandler;

public partial class ServiceHandler
{
  public async Task<Response<object>> HandleUnasignResponsabilityAsync(Guid id, Guid responsabilityId)
  {
    var service = await _repository.GetServiceIncludeResponsabilities(id);
    if (service == null)
      throw new ArgumentException("Serviço não encontrado.");

    var responsability = await _responsabilityRepository.GetEntityByIdAsync(responsabilityId);
    if (responsability == null)
      throw new ArgumentException("Responsabilidade não encontrada.");

    service.RemoveResponsability(responsability);

    await _repository.SaveChangesAsync();

    return new Response<object>(200, "Responsabilidade desatribuida.");
  }
}