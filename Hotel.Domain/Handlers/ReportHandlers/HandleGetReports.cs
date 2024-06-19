using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ReportDTOs;

namespace Hotel.Domain.Handlers.ReportHandlers;

public partial class ReportHandler
{
    public async Task<Response<IEnumerable<GetReport>>> HandleGetAsync(ReportQueryParameters queryParameters)
    {
        var reports = await _repository.GetAsync(queryParameters);
        return new Response<IEnumerable<GetReport>>("Sucesso!", reports);
    }
}