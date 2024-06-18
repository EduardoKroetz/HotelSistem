using Hotel.Domain.Entities.VerificationCodeEntity;
using Hotel.Domain.Repositories.Interfaces;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Services.EmailServices;

public partial class EmailService
{
    private readonly IVerificationCodeRepository _verificationCodeRepository;

    public EmailService(IVerificationCodeRepository verificationCodeRepository)
    => _verificationCodeRepository = verificationCodeRepository;

    public async Task VerifyEmailCodeAsync(Email email, string? codeStr)
    {
        if (string.IsNullOrEmpty(codeStr))
            throw new ArgumentException("Código inválido.");

        var code = new VerificationCode(codeStr);
        var verificationCode = await _verificationCodeRepository.GetCode(code);

        if (verificationCode == null || verificationCode.Email == null || verificationCode?.Email.Address != email.Address)
            throw new ArgumentException("Código inválido.");

        //Deletar código após ser validado
        _verificationCodeRepository.Delete(verificationCode);
        await _verificationCodeRepository.SaveChangesAsync();
    }

}
