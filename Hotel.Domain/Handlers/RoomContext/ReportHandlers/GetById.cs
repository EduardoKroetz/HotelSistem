using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.ReportDTOs;

namespace Hotel.Domain.Handlers.RoomContext.ReportHandlers;

public partial class ReportHandler
{
  public async Task<Response<GetReport>> HandleGetByIdAsync(Guid id)
  {
    var report = await _repository.GetByIdAsync(id);
    if (report == null)
      throw new ArgumentException("Relatório não encontrado.");
    
    return new Response<GetReport>(200, "Sucesso!", report);
  }
}