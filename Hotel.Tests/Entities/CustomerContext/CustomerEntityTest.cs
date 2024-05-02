using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects;

namespace Hotel.Tests.Entities.CustomerContext;

[TestClass]
public class CustomerEntityTest 
{
  [TestMethod]
  public void CreateCustomer_With_ValidParameters_MustBeValid()
  {
    var customer = new Customer(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    Assert.AreEqual(true, customer.IsValid);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void CreateCustomer_With_InvalidParameters_ExpectedException()
  {
    new Customer(new Name("",""),new Email(""),new Phone(""),"");
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void AddInvalidFeedback_ExpectedException()
  {
    var customer = new Customer(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    new Feedback("",10, customer,TestParameters.Reservation,TestParameters.Room); //AddFeedback está no construtor de feedback
  }

  [TestMethod]
  public void AddValidFeedback_MustBeValid()
  {
    var customer = new Customer(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    new Feedback("Feedback example",10, customer,TestParameters.Reservation,TestParameters.Room);
    Assert.AreEqual(1,customer.Feedbacks.Count);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void AddFeedback_With_AlreadyAdded_ExpectedExpection()
  {
    var customer = new Customer(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    var feedback = new Feedback("Feedback example",10, customer,TestParameters.Reservation,TestParameters.Room);
    customer.AddFeedback(feedback);
    Assert.Fail();
  }

  [TestMethod]
  public void RemoveFeedback_With_ContainsFeedback_MustBeValid()
  {
    var customer = new Customer(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    var feedback = new Feedback("Feedback example",10, customer,TestParameters.Reservation,TestParameters.Room);
    customer.RemoveFeedback(feedback);
    Assert.AreEqual(0,customer.Feedbacks.Count);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void RemoveFeedback_Without_ContainsFeedback_ExpectedException()
  {
    var customer = new Customer(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    var feedback = new Feedback("Feedback example",10, TestParameters.Customer,TestParameters.Reservation,TestParameters.Room);
    customer.RemoveFeedback(feedback);
    Assert.Fail();
  }




}