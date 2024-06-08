using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.ReportDTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomContext.ReportHandlers;

public partial class ReportHandler
{
  public async Task<Response<GetReport>> HandleGetByIdAsync(Guid id)
  {
    var report = await _repository.GetByIdAsync(id)
      ?? throw new NotFoundException("Relatório não encontrado.");

    return new Response<GetReport>(200, "Sucesso!", report);
  }
}