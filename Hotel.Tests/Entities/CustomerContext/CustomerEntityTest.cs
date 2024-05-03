using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;

namespace Hotel.Tests.Entities.CustomerContext;

[TestClass]
public class CustomerEntityTest 
{
  [TestMethod]
  public void ValidCustomer_MustBeValid()
  {
    var customer = new Customer(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    Assert.IsTrue(customer.IsValid);
  }

  [TestMethod]
  public void AddFeedback_MustBeAdded()
  {
    var customer = new Customer(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    var feedback = new Feedback("Feedback example",10, customer,TestParameters.Reservation,TestParameters.Room);
    customer.AddFeedback(feedback);
    Assert.AreEqual(1,customer.Feedbacks.Count);
  }

  [TestMethod]
  public void AddSameFeedback_AddJustOne()
  {
    var customer = new Customer(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    var feedback = new Feedback("Feedback example",10, customer,TestParameters.Reservation,TestParameters.Room);
    customer.AddFeedback(feedback);
    customer.AddFeedback(feedback);
    Assert.AreEqual(1, customer.Feedbacks.Count);
  }

  [TestMethod]
  public void RemoveFeedback_MustBeRemoved()
  {
    var customer = new Customer(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    var feedback = new Feedback("Feedback example",10, customer,TestParameters.Reservation,TestParameters.Room);
    customer.AddFeedback(feedback);
    customer.RemoveFeedback(feedback);
    Assert.AreEqual(0,customer.Feedbacks.Count);
  }

  [TestMethod]
  public void RemoveNonExistingFeedback_DoNothing()
  {
    var customer = new Customer(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    var feedback = new Feedback("Feedback example",10, TestParameters.Customer,TestParameters.Reservation,TestParameters.Room);
    customer.RemoveFeedback(feedback);
    Assert.AreEqual(0, customer.Feedbacks.Count);
  }




}