using Hotel.Domain.Entities.VerificationCodeEntity;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IVerificationCodeRepository
{
    Task CreateAsync(VerificationCode verificationCode);
    void Delete(VerificationCode verificationCode);
    Task<VerificationCode?> GetCode(VerificationCode code);
    Task RemoveEmailAlreadyExists(Email email);
    Task SaveChangesAsync();
}
