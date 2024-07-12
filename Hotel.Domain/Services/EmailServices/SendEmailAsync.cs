using Hotel.Domain.Services.EmailServices.Interfaces;
using Hotel.Domain.Services.EmailServices.Models;
using System.Net;
using System.Net.Mail;

namespace Hotel.Domain.Services.EmailServices;

public partial class EmailService : IEmailService
{
    public async Task SendEmailAsync(SendEmailModel email)
    {
        var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(Configuration.EmailToSendEmail);
        mailMessage.Subject = email.Subject;
        mailMessage.Body = email.Body;
        mailMessage.To.Add(email.To.Address);

        if (email.AttachmentPath != null)
            mailMessage.Attachments.Add(new Attachment(email.AttachmentPath));

        var smtpClient = new SmtpClient("smtp-mail.outlook.com", 587)
        {
            DeliveryMethod = SmtpDeliveryMethod.Network,
            EnableSsl = true,
            Credentials = new NetworkCredential(Configuration.EmailToSendEmail, Configuration.PasswordToSendEmail)
        };
  
        await smtpClient.SendMailAsync(mailMessage);
        _logger.LogInformation($"Email enviado com sucesso para {email.To.Address}");
    }
}
