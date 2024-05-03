using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;


namespace Hotel.Domain.Entities.CustomerContext;

public partial class Customer 
{
  public void AddFeedback(Feedback feedback)
  => Feedbacks.Add(feedback); 
  
  public void RemoveFeedback(Feedback feedback)
  => Feedbacks.Remove(feedback);
}