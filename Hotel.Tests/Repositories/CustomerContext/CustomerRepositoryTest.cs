using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.CustomerContext;
using Hotel.Tests.Repositories.Mock;

namespace Hotel.Tests.Repositories.CustomerContext;

[TestClass]
public class CustomerRepositoryTest
{
  private static CustomerRepository CustomerRepository { get; set; }

  static CustomerRepositoryTest()
  => CustomerRepository = new CustomerRepository(BaseRepositoryTest.MockConnection.Context);

  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var customer = await CustomerRepository.GetByIdAsync(BaseRepositoryTest.Customers[0].Id);

    Assert.IsNotNull(customer);
    Assert.AreEqual(BaseRepositoryTest.Customers[0].Name.FirstName, customer.FirstName);
    Assert.AreEqual(BaseRepositoryTest.Customers[0].Name.LastName, customer.LastName);
    Assert.AreEqual(BaseRepositoryTest.Customers[0].Email.Address, customer.Email);
    Assert.AreEqual(BaseRepositoryTest.Customers[0].Phone.Number, customer.Phone);
    Assert.AreEqual(BaseRepositoryTest.Customers[0].Id, customer.Id);
  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new UserQueryParameters(0, 1, BaseRepositoryTest.Customers[0].Name.FirstName, null, null, null, null, null, null, null);
    var customers = await CustomerRepository.GetAsync(parameters);

    var customer = customers.ToList()[0];

    Assert.IsNotNull(customer);
    Assert.AreEqual(BaseRepositoryTest.Customers[0].Name.FirstName, customer.FirstName);
    Assert.AreEqual(BaseRepositoryTest.Customers[0].Name.LastName, customer.LastName);
    Assert.AreEqual(BaseRepositoryTest.Customers[0].Email.Address, customer.Email);
    Assert.AreEqual(BaseRepositoryTest.Customers[0].Phone.Number, customer.Phone);
    Assert.AreEqual(BaseRepositoryTest.Customers[0].Id, customer.Id);
  }

  [TestMethod]
  public async Task GetAsync_WhereNameIncludesJoao_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 1, "João", null, null, null, null, null, null, null);
    var customers = await CustomerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());
    foreach (var customer in customers)
      Assert.IsTrue(customer.FirstName.Contains("João"));

  }

  [TestMethod]
  public async Task GetAsync_WhereEmailContainsCom_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, null, ".com", null, null, null, null, null, null);
    var customers = await CustomerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());
    foreach (var customer in customers)
      Assert.IsTrue(customer.Email.Contains(".com"));
   
  }

  [TestMethod]
  public async Task GetAsync_WherePhoneContains55_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, null, null, "55", null, null, null, null, null);
    var customers = await CustomerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());
    foreach (var customer in customers)
      Assert.IsTrue(customer.Phone.Contains("55"));
    
  }

  [TestMethod]
  public async Task GetAsync_WhereGenderEqualsMasculine_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, null, null, null, EGender.Masculine, null, null, null, null);
    var customers = await CustomerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());
    foreach (var customer in customers)
      Assert.AreEqual(EGender.Masculine, customer.Gender);
    
  }

  [TestMethod]
  public async Task GetAsync_WhereDateOfBirthGratherThan2000_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, null, null, null, null, DateTime.Now.AddYears(-24), "gt", null, null);
    var customers = await CustomerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());
    foreach (var customer in customers)
      Assert.IsTrue(DateTime.Now.AddYears(-24) < customer.DateOfBirth);
    
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtGratherThanYesterday_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, null, null, null, null, null, null, DateTime.Now.AddDays(-1), "gt");
    var customers = await CustomerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());
    foreach (var customer in customers)
      Assert.IsTrue(DateTime.Now.AddDays(-1) < customer.CreatedAt);
    
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtLessThanToday_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, null, null, null, null, null, null, DateTime.Now.AddDays(1), "lt");
    var customers = await CustomerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());
    foreach (var customer in customers)
      Assert.IsTrue(DateTime.Now.AddDays(1) > customer.CreatedAt);
    
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtEquals_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, null, null, null, null, null, null, BaseRepositoryTest.Customers[0].CreatedAt, "eq");
    var customers = await CustomerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());
    foreach (var customer in customers)
      Assert.AreEqual(BaseRepositoryTest.Customers[0].CreatedAt, customer.CreatedAt);
    
  }

  [TestMethod]
  public async Task GetAsync_WhereEmailIncludesExample_And_PhoneIncludes55_And_GenderEqualsMasculine_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, null, "example", "55", EGender.Masculine, null, null, null, null);
    var customers = await CustomerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());

    foreach (var customer in customers)
    { 
      Assert.IsTrue(customer.Email.Contains("example"));
      Assert.IsTrue(customer.Phone.Contains("55"));
      Assert.AreEqual(EGender.Masculine, customer.Gender);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereNameIncludesR_And_DateOfBirthLessThan31Years_And_CreatedAtLessThanTomorrow_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, "R", null, null, null, DateTime.Now.AddYears(-31), "lt", DateTime.Now.AddDays(1), "lt");

    var customers = await CustomerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());

    foreach (var customer in customers)
    { 
      Assert.IsTrue(customer.FirstName.Contains('R'));
      Assert.IsTrue(DateTime.Now.AddYears(-31) > customer.DateOfBirth);
      Assert.IsTrue(DateTime.Now.AddDays(1) > customer.CreatedAt);
    }
  }
}
