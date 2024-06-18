using Hotel.Domain.Entities.FeedbackEntity;


namespace Hotel.Domain.Entities.CustomerEntity;

public partial class Customer
{
    public void AddFeedback(Feedback feedback)
    => Feedbacks.Add(feedback);

    public void RemoveFeedback(Feedback feedback)
    => Feedbacks.Remove(feedback);
}