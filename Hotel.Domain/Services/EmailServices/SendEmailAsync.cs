using System.Net.Mail;
using System.Net;
using Hotel.Domain.Services.EmailServices.Models;
using Hotel.Domain.Services.EmailServices.Interface;

namespace Hotel.Domain.Services.EmailServices;

public partial class EmailService : IEmailService
{
  public async Task SendEmailAsync(SendEmailModel email)
  {
    var mailMessage = new MailMessage();
    mailMessage.From = new MailAddress(Configuration.Configuration.EmailToSendEmail);
    mailMessage.Subject = email.Subject;
    mailMessage.Body = email.Body;
    mailMessage.To.Add(email.To.Address);

    var smtpClient = new SmtpClient("smtp-mail.outlook.com", 587)
    {
      DeliveryMethod = SmtpDeliveryMethod.Network,
      EnableSsl = true,
      Credentials = new NetworkCredential(Configuration.Configuration.EmailToSendEmail, Configuration.Configuration.PasswordToSendEmail)
    };

    try
    {
      await smtpClient.SendMailAsync(mailMessage);
      Console.WriteLine("Email enviado com sucesso!");
    }catch (Exception ex)
    {
      Console.WriteLine("Ocorreu um erro ao enviar o e-mail: " + ex.Message);
    }
  }
}
