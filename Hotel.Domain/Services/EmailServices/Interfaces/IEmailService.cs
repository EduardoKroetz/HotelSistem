using Hotel.Domain.Services.EmailServices.Models;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Services.EmailServices.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(SendEmailModel email);
    Task VerifyEmailCodeAsync(Email email, string? codeStr);
}
