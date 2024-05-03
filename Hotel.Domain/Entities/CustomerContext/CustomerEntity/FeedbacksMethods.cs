

using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.CustomerContext;

public partial class Customer 
{
  public void AddFeedback(Feedback feedback)
  {
    if (Feedbacks.Contains(feedback))
      throw new ValidationException("Erro de validação: Esse feedback já foi adicionado.");
    else
      Feedbacks.Add(feedback); 
  }

  public void RemoveFeedback(Feedback feedback)
  {
    if (!Feedbacks.Remove(feedback))
      throw new ValidationException("Erro de validação: Feedback não encontrado.");
  }
}