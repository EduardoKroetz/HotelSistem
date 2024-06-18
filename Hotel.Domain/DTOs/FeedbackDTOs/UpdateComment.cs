namespace Hotel.Domain.DTOs.FeedbackDTOs;

public class UpdateComment
{
    public UpdateComment(string comment)
    => Comment = comment;

    public string Comment { get; private set; }
}
