using Hotel.Domain.Entities.VerificationCodeEntity;
using Hotel.Domain.Repositories.Interfaces;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Services.EmailServices;

public partial class EmailService
{
    private readonly IVerificationCodeRepository _verificationCodeRepository;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IVerificationCodeRepository verificationCodeRepository, ILogger<EmailService> logger)
    {
        _verificationCodeRepository = verificationCodeRepository;
        _logger = logger;
    }

    public async Task VerifyEmailCodeAsync(Email email, string? codeStr)
    {
        if (string.IsNullOrEmpty(codeStr))
        {
            _logger.LogWarning($"Código de validação de email é nullo ou vazio. Email: {email.Address}");
            throw new ArgumentException("Código inválido.");
        }

        var code = new VerificationCode(codeStr);
        var verificationCode = await _verificationCodeRepository.GetCode(code);

        if (verificationCode == null || verificationCode.Email == null || verificationCode?.Email.Address != email.Address)
        {
            _logger.LogWarning($"Código de validação de email é inválido. Email: {email.Address}");
            throw new ArgumentException("Código inválido.");
        }

        //Deletar código após ser validado
        _verificationCodeRepository.Delete(verificationCode);
        await _verificationCodeRepository.SaveChangesAsync();
    }

}
