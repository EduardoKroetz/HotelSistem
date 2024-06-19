using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ResponsibilityDTOs;

namespace Hotel.Domain.Handlers.y.ResponsibilityHandlers;

public partial class ResponsibilityHandler
{
    public async Task<Response<GetResponsibility>> HandleGetByIdAsync(Guid id)
    {
        var responsibility = await _repository.GetByIdAsync(id);
        if (responsibility == null)
            throw new ArgumentException("Responsabilidade não encontrada.");

        return new Response<GetResponsibility>("Responsabilidade encontrada com sucesso!.", responsibility);
    }
}