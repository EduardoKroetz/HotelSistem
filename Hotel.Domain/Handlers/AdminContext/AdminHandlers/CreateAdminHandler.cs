
using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;

public class CreateAdminHandler : IHandler<CreateAdmin, Response<object>>
{
  private readonly IAdminRepository _repository;
  public CreateAdminHandler(IAdminRepository repository)
  => _repository = repository;

  public async Task<Response<object>> HandleAsync(CreateAdmin dto)
  {
    var admin = new Admin(
      new Name(dto.FirstName,dto.LastName),
      new Email(dto.Email),
      new Phone(dto.Phone),
      dto.Password,
      dto.Gender,
      dto.DateOfBirth,
      new Address(dto.Country,dto.City,dto.Street,dto.Number ?? 0)
    );

    _repository.Create(admin);

    return new Response<object>(200,"Admin criado com sucesso!",new { admin.Id });
  }
}