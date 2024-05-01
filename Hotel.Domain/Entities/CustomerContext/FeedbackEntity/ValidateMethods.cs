using System.ComponentModel.DataAnnotations;

namespace Hotel.Domain.Entities.CustomerContext.FeedbackEntity;

public partial class Feedback
{

  public override void Validate()
  {
    Room?.Validate();
    Reservation?.Validate();
    Customer?.Validate();

    if (Rate > 10)
      throw new ValidationException("Informe uma avaliação válida.");
    if (Comment.Length > 1000)
      throw new ValidationException("Limite máximo de 1000 caracteres por comentário foi atingido.");
    
    base.Validate();
  }
}