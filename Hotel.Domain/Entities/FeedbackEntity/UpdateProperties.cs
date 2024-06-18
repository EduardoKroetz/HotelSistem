namespace Hotel.Domain.Entities.FeedbackEntity;

public partial class Feedback
{
    public void ChangeComment(string comment)
    {
        ValidateComment(comment);
        Comment = comment;
    }


    public void ChangeRate(int rate)
    {
        ValidateRate(rate);
        Rate = rate;
    }

}