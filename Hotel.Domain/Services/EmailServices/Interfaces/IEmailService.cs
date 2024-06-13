﻿using Hotel.Domain.Services.EmailServices.Models;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Services.EmailServices.Interface;

public interface IEmailService
{
  Task SendEmailAsync(SendEmailModel email);
  Task VerifyEmailCodeAsync(Email email, string? codeStr);
}
