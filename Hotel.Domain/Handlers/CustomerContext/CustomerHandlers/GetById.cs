using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.User;

namespace Hotel.Domain.Handlers.CustomerContext.CustomerHandlers;

public partial class CustomerHandler 
{
  public async Task<Response<GetUser>> HandleGetByIdAsync(Guid id)
  {
    var permission = await _repository.GetByIdAsync(id);
    if (permission == null)
      throw new ArgumentException("Cliente não encontrado.");
    
    return new Response<GetUser>(200,"Cliente encontrado com sucesso!", permission);
  }
}