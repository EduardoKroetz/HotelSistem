using Hotel.Domain.Entities.FeedbackEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Tests.UnitTests.Entities;

[TestClass]
public class FeedbackEntityTest
{
    [TestMethod]
    public void NewFeedbackInstance_MustBeValid()
    {
        var feedback = new Feedback("Feedback", 5, TestParameters.Customer.Id, TestParameters.Reservation.Id, TestParameters.Room.Id);
        Assert.IsTrue(feedback.IsValid);
    }
}
