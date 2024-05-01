namespace Hotel.Domain.Entities.CustomerContext.FeedbackEntity;

public partial class Feedback
{
  public void ChangeComment(string comment)
  => Comment = comment;
  
  public void ChangeRate(string comment)
  => Comment = comment;
}