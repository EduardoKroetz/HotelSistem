namespace Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;

public class UpdateComment
{
  public UpdateComment(string comment)
  => Comment = comment;   
  
  public string Comment { get; private set; }
}
