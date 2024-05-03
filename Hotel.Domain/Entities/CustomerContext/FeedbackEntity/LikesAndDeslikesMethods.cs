namespace Hotel.Domain.Entities.CustomerContext.FeedbackEntity;

public partial class Feedback
{
  public void AddLike()
  => Likes++;

  public void RemoveLike()
  {
    if (Likes > 0)
      Likes--; 
  }
  
  public void AddDeslike()
  => Deslikes++; 
  public void RemoveDeslike()
   {
    if (Deslikes > 0)
      Deslikes--;  
  }


}