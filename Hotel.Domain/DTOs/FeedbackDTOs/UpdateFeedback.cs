namespace Hotel.Domain.DTOs.FeedbackDTOs;

public class UpdateFeedback : IDataTransferObject
{
    public UpdateFeedback(string comment, int rate)
    {
        Comment = comment;
        Rate = rate;
    }

    public string Comment { get; private set; }
    public int Rate { get; private set; }
}

