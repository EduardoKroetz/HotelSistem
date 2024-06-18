using Hotel.Domain.Entities.CustomerEntity;

namespace Hotel.Tests.UnitTests.Entities;

[TestClass]
public class CustomerEntityTest
{
    [TestMethod]
    public void ValidCustomer_MustBeValid()
    {
        var customer = new Customer(TestParameters.Name, TestParameters.Email, TestParameters.Phone, "password123");
        Assert.IsTrue(customer.IsValid);
    }
}