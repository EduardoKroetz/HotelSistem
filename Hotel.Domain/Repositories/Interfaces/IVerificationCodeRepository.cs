using Hotel.Domain.Entities;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IVerificationCodeRepository
{
  Task CreateAsync(VerificationCode verificationCode);
  void Delete(VerificationCode verificationCode);
  Task<VerificationCode?> GetCode(VerificationCode code);
  Task SaveChangesAsync();
}
