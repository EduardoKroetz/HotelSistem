using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomContext.ReportHandlers;

public partial class ReportHandler
{
  public async Task<Response> HandleCancelAsync(Guid id)
  {
    var report = await _repository.GetEntityByIdAsync(id)
      ?? throw new NotFoundException("Relatório não encontrado.");

    report.Cancel();

    await _repository.SaveChangesAsync();
    return new Response(200, "Relatório cancelado com sucesso!.");
  }
}