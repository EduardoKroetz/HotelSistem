using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.FeedbackEntity;

namespace Hotel.Domain.Entities.LikeEntity;

public class Like : Entity
{
    internal Like() { }
    public Like(Customer customer, Feedback feedback)
    {
        Customer = customer;
        Feedback = feedback;
    }

    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public Guid FeedbackId { get; private set; }
    public Feedback Feedback { get; private set; } = null!;
}
