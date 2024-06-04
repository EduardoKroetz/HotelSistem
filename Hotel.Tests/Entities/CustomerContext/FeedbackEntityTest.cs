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
}