using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.Base.User;

namespace Hotel.Domain.Handlers.CustomerHandlers;

public partial class CustomerHandler
{
    public async Task<Response<IEnumerable<GetUser>>> HandleGetAsync(UserQueryParameters queryParameters)
    {
        var customers = await _repository.GetAsync(queryParameters);
        return new Response<IEnumerable<GetUser>>(200, "Sucesso!", customers);
    }
}