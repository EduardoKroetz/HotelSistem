﻿using Hotel.Domain.DTOs;
using Hotel.Domain.Entities;
using Hotel.Domain.Entities.VerificationCodeEntity;
using Hotel.Domain.Handlers.Base.Interfaces;
using Hotel.Domain.Repositories.Interfaces;
using Hotel.Domain.Services.EmailServices.Interfaces;
using Hotel.Domain.Services.EmailServices.Models;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.VerificationHandlers;

public partial class VerificationHandler : IHandler
{
    private readonly IVerificationCodeRepository _verificationCodeRepository;
    private readonly IEmailService _emailService;

    public VerificationHandler(IVerificationCodeRepository verificationCodeRepository, IEmailService emailService)
    {
        _verificationCodeRepository = verificationCodeRepository;
        _emailService = emailService;
    }

    public async Task<Response> HandleSendEmailCodeAsync(string? to)
    {
        var toEmail = new Email(to);
        var code = new VerificationCode(toEmail);

        await _verificationCodeRepository.RemoveEmailAlreadyExists(toEmail); //Verifica se existe um código atribuido a esse email e delete ele
        await _verificationCodeRepository.CreateAsync(code);
        await _verificationCodeRepository.SaveChangesAsync();

        var email = new SendEmailModel(
          toEmail,
          "Validar o email",
          $"Código de validação: {code.Code}"
        );

        await _emailService.SendEmailAsync(email);

        return new Response("Código de validação enviado com sucesso!");
    }
}
