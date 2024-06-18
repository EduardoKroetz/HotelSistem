using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Services.EmailServices.Models;

public class SendEmailModel
{
    public SendEmailModel(Email to, string subject, string body, string? attachmentPath = null)
    {
        To = to;
        Subject = subject;
        Body = body;
        AttachmentPath = attachmentPath;
    }

    public Email To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string? AttachmentPath { get; private set; }

}
