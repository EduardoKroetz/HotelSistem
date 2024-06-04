using Hotel.Domain.Entities.CustomerContext;

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
    customer.AddFeedback(TestParameters.Feedback);
    Assert.AreEqual(1,customer.Feedbacks.Count);
  }

  [TestMethod]
  public void AddSameFeedback_AddJustOne()
  {
    var customer = new Customer(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    customer.AddFeedback(TestParameters.Feedback);
    customer.AddFeedback(TestParameters.Feedback);
    Assert.AreEqual(1, customer.Feedbacks.Count);
  }

  [TestMethod]
  public void RemoveFeedback_MustBeRemoved()
  {
    var customer = new Customer(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    customer.AddFeedback(TestParameters.Feedback);
    customer.RemoveFeedback(TestParameters.Feedback);
    Assert.AreEqual(0,customer.Feedbacks.Count);
  }

  [TestMethod]
  public void RemoveNonExistingFeedback_DoNothing()
  {
    var customer = new Customer(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    customer.RemoveFeedback(TestParameters.Feedback);
    Assert.AreEqual(0, customer.Feedbacks.Count);
  }




}