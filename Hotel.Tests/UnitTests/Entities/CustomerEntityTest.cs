using Hotel.Domain.Entities.CustomerEntity;

namespace Hotel.Tests.UnitTests.Entities;

[TestClass]
public class CustomerEntityTest
{
    [TestMethod]
    public void NewCustomerInstance_MustBeValid()
    {
        var customer = new Customer(TestParameters.Name, TestParameters.Email, TestParameters.Phone, "password123");

        Assert.IsTrue(customer.IsValid);
        Assert.AreEqual(TestParameters.Name, customer.Name);
        Assert.AreEqual(TestParameters.Email, customer.Email);
        Assert.AreEqual(TestParameters.Phone, customer.Phone);
        Assert.AreNotEqual("password123", customer.PasswordHash);
        Assert.AreEqual(0, customer.Feedbacks.Count);
        Assert.AreEqual(0, customer.Invoices.Count);
        Assert.AreEqual(0, customer.Reservations.Count);
        Assert.AreEqual(0, customer.Dislikes.Count);
        Assert.AreEqual(0, customer.Likes.Count);
        Assert.AreEqual("", customer.StripeCustomerId);
        Assert.IsNull(customer.Address);
        Assert.IsNull(customer.DateOfBirth);
        Assert.IsTrue(customer.IncompleteProfile);
    }
}