using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.RoomContext.ServiceHandler;

public partial class ServiceHandler
{
  public async Task<Response> HandleAssignResponsabilityAsync(Guid id, Guid responsabilityId)
  {
    var service = await _repository.GetServiceIncludeResponsabilities(id);
    if (service == null)
      throw new ArgumentException("Serviço não encontrado.");

    var responsability = await _responsabilityRepository.GetEntityByIdAsync(responsabilityId);
    if (responsability == null)
      throw new ArgumentException("Responsabilidade não encontrada.");

    service.AddResponsability(responsability);

    await _repository.SaveChangesAsync();

    return new Response(200, "Responsabilidade atribuida com sucesso!");
  }
}