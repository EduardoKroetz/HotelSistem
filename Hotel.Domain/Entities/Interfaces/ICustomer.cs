using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;

namespace Hotel.Domain.Entities.Interfaces;

public interface ICustomer : IUser
{
  HashSet<Feedback> Feedbacks { get; }
}
