using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ResponsibilityDTOs;

namespace Hotel.Domain.Handlers.y.ResponsibilityHandlers;

public partial class ResponsibilityHandler
{
    public async Task<Response<IEnumerable<GetResponsibility>>> HandleGetAsync(ResponsibilityQueryParameters queryParameters)
    {
        var responsibilities = await _repository.GetAsync(queryParameters);
        return new Response<IEnumerable<GetResponsibility>>("Sucesso!", responsibilities);
    }
}