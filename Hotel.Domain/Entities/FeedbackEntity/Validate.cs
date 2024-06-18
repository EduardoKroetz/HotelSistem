using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.FeedbackEntity;

public partial class Feedback
{

    public override void Validate()
    {
        ValidateComment(Comment);
        ValidateRate(Rate);

        base.Validate();
    }

    public void ValidateRate(int rate)
    {
        if (rate > 10 || rate < 0)
            throw new ValidationException("Informe uma avaliação válida.");
    }

    public void ValidateComment(string comment)
    {
        if (string.IsNullOrEmpty(comment))
            throw new ValidationException("Informe o comentário do feedback.");
        if (comment.Length > 500)
            throw new ValidationException("Limite máximo de 500 caracteres por comentário foi atingido.");
    }

}