using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Tests.Entities.CustomerContext;

[TestClass]
public class FeedbackEntityTest
{
  [TestMethod]
  public void ValidFeedback_MustBeValid()
  {
    var feedback = new Feedback("Feedback",5,TestParameters.Customer.Id,TestParameters.Reservation.Id,TestParameters.Room.Id);
    Assert.IsTrue(feedback.IsValid);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  [DataRow("",1)]
  [DataRow("Feedback",-1)]
  [DataRow("Feedback",11)]
  [DataRow(TestParameters.DescriptionMaxCaracteres,5)]
  public void InvalidFeedback_ExpectedException(string comment, int rate)
  {
    new Feedback(comment,rate,TestParameters.Customer.Id,TestParameters.Reservation.Id,TestParameters.Room.Id);
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  [DataRow("",1)]
  [DataRow("Feedback",-1)]
  [DataRow("Feedback",11)]
  [DataRow(TestParameters.DescriptionMaxCaracteres,5)]
  public void ChangeToInvalidFeedback_ExpectedException(string comment, int rate)
  {
    var feedback = new Feedback(comment,rate,TestParameters.Customer.Id,TestParameters.Reservation.Id,TestParameters.Room.Id);
    feedback.ChangeComment(comment);
    feedback.ChangeRate(rate);
    Assert.Fail();
  }

  [TestMethod]
  public void AddLike_MustBeAdded()
  {
    var feedback = new Feedback("Feedback",3,TestParameters.Customer.Id,TestParameters.Reservation.Id,TestParameters.Room.Id);
    feedback.AddLike();
    Assert.AreEqual(1,feedback.Likes);
  }

  [TestMethod]
  public void RemoveLike_WithoutLikes_MustBe0Likes()
  {
    var feedback = new Feedback("Feedback",3,TestParameters.Customer.Id,TestParameters.Reservation.Id,TestParameters.Room.Id);
    feedback.RemoveLike();
    Assert.AreEqual(0,feedback.Likes);
  }


  [TestMethod]
  public void AddDeslike_MustBeAdded()
  {
    var feedback = new Feedback("Feedback",3,TestParameters.Customer.Id,TestParameters.Reservation.Id,TestParameters.Room.Id);
    feedback.AddDeslike();
    Assert.AreEqual(1,feedback.Deslikes);
  }

  [TestMethod]
  public void RemoveDeslike_WithoutDeslikes_MustBe0Deslikes()
  {
    var feedback = new Feedback("Feedback",3,TestParameters.Customer.Id,TestParameters.Reservation.Id,TestParameters.Room.Id);
    feedback.RemoveDeslike();
    Assert.AreEqual(0,feedback.Likes);
  }
}