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

    [TestMethod]
    [DataRow("", 1, "Informe o comentário do feedback")]
    [DataRow("Feedback", -1, "A avaliação deve ser entre 1 e 10")]
    [DataRow("Feedback", 11, "A avaliação deve ser entre 1 e 10")]
    public void InvalidFeedback_ShouldThrowException(string comment, int rate, string expectedMessage)
    {
        var exception = Assert.ThrowsException<ValidationException>(() => new Feedback(comment, rate, TestParameters.Customer.Id, TestParameters.Reservation.Id, TestParameters.Room.Id));
        Assert.AreEqual(expectedMessage, exception.Message);
    }
}
