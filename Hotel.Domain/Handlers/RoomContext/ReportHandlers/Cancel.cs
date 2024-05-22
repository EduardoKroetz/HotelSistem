using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.RoomContext.ReportHandlers;

public partial class ReportHandler
{
  public async Task<Response<object>> HandleCancelAsync(Guid id)
  {
    var report = await _repository.GetEntityByIdAsync(id);
    if (report == null)
      throw new ArgumentException("Relatório não encontrado.");

    report.Cancel();

    await _repository.SaveChangesAsync();
    return new Response<object>(200, "Relatório cancelado.");
  }
}