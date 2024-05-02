using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Tests.Entities.CustomerContext;

[TestClass]
public class FeedbackEntityTest
{
  [TestMethod]
  public void CreateValidFeedback_MustBeValid()
  {
    var feedback = new Feedback("Feedback",5,TestParameters.Customer,TestParameters.Reservation,TestParameters.Room);
    Assert.AreEqual(true, feedback.IsValid);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  [DataRow("",1)]
  [DataRow("Feedback",-1)]
  [DataRow("Feedback",11)]
  [DataRow(TestParameters.DescriptionMaxCaracteres,5)]
  public void CreateInvalidFeedback_ExpectedException(string comment, int rate)
  {
    new Feedback(comment,rate,TestParameters.Customer,TestParameters.Reservation,TestParameters.Room);
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  [DataRow("",1)]
  [DataRow("Feedback",-1)]
  [DataRow("Feedback",11)]
  [DataRow(TestParameters.DescriptionMaxCaracteres,5)]
  public void ChangeFeedback_WithInvalidParameters_ExpectedException(string comment, int rate)
  {
    var feedback = new Feedback(comment,rate,TestParameters.Customer,TestParameters.Reservation,TestParameters.Room);
    feedback.ChangeComment(comment);
    feedback.ChangeRate(rate);
    Assert.Fail();
  }

  [TestMethod]
  public void AddLike_MustBeAdded()
  {
    var feedback = new Feedback("Feedback",3,TestParameters.Customer,TestParameters.Reservation,TestParameters.Room);
    feedback.AddLike();
    Assert.AreEqual(1,feedback.Likes);
  }

  [TestMethod]
  public void RemoveLike_Without_ContainLikes_MustBe0Likes()
  {
    var feedback = new Feedback("Feedback",3,TestParameters.Customer,TestParameters.Reservation,TestParameters.Room);
    feedback.RemoveLike();
    Assert.AreEqual(0,feedback.Likes);
  }


  [TestMethod]
  public void AddDeslike_MustBeAdded()
  {
    var feedback = new Feedback("Feedback",3,TestParameters.Customer,TestParameters.Reservation,TestParameters.Room);
    feedback.AddDeslike();
    Assert.AreEqual(1,feedback.Deslikes);
  }

  [TestMethod]
  public void RemoveDeslike_Without_ContainDeslikes_MustBe0Deslikes()
  {
    var feedback = new Feedback("Feedback",3,TestParameters.Customer,TestParameters.Reservation,TestParameters.Room);
    feedback.RemoveDeslike();
    Assert.AreEqual(0,feedback.Likes);
  }
}