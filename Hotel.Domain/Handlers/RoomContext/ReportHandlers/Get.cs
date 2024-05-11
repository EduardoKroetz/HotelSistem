using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.ReportDTOs;

namespace Hotel.Domain.Handlers.RoomContext.ReportHandlers;

public partial class ReportHandler
{
  public async Task<Response<IEnumerable<GetReport>>> HandleGetAsync()
  {
    var reports = await _repository.GetAsync();
    return new Response<IEnumerable<GetReport>>(200,"", reports);
  }
} 