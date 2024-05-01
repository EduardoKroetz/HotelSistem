namespace Hotel.Domain.Entities.CustomerContext.FeedbackEntity;

public partial class Feedback
{
  public void AddLike()
  => Likes++; 
  public void RemoveLike()
  => Likes--; 
  
  public void AddDeslike()
  => Deslikes++; 
  public void RemoveDeslike()
  => Deslikes--; 

}