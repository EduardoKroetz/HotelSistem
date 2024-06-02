using Hotel.Domain.DTOs;
using Hotel.Domain.Services.EmailServices.Models;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Services.EmailServices.Interface;

public interface IEmailService
{
  Task SendEmailAsync(SendEmailModel email);
  Task<Response> VerifyEmailCodeAsync(Email email, string? codeStr);
}
