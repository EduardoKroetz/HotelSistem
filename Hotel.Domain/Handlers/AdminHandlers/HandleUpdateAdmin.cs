using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Handlers.AdminHandlers;

public partial class AdminHandler
{
    public async Task<Response> HandleUpdateAsync(UpdateUser model, Guid adminId)
    {
        var admin = await _repository.GetEntityByIdAsync(adminId)
          ?? throw new NotFoundException("Administrador não encontrado.");

        admin.ChangeName(new Name(model.FirstName, model.LastName));
        admin.ChangePhone(new Phone(model.Phone));
        admin.ChangeGender(model.Gender);
        admin.ChangeDateOfBirth(model.DateOfBirth);
        admin.ChangeAddress(new Address(model.Country, model.City, model.Street, model.Number));

        try
        {
            _repository.Update(admin);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            var innerException = e.InnerException?.Message;

            if (innerException != null)
            {

                if (innerException.Contains("Email"))
                {
                    _logger.LogError("Erro ao cadastradar administrador pois o email já está cadastrado");
                    throw new ArgumentException("Esse email já está cadastrado.");
                }

                if (innerException.Contains("Phone"))
                {
                    _logger.LogError("Erro ao cadastradar administrador pois o telefone já está cadastrado");
                    throw new ArgumentException("Esse telefone já está cadastrado.");
                }
            }
        }


        return new Response("Administrador atualizado com sucesso!");
    }
}