using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Services.EmailServices.Models;

public class SendEmailModel
{
  public SendEmailModel(Email to, string subject, string body)
  {
    To = to;
    Subject = subject;
    Body = body;
  }

  public Email To { get; set; }
  public string Subject { get; set; }
  public string Body { get; set; }

}
