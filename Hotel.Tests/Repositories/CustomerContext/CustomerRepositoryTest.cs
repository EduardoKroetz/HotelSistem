using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.CustomerContext;
using Hotel.Domain.ValueObjects;

namespace Hotel.Tests.Repositories.CustomerContext;

[TestClass]
public class CustomerRepositoryTest : BaseRepositoryTest
{
  private List<Customer> _customers { get; set; } = [];
  private Customer _defaultCustomer { get; set; } = null!;
  private CustomerRepository _customerRepository { get; set; } = null!;

  [TestInitialize]
  public async Task StartupTest()
  {
    await Startup(null);
    _customerRepository = new CustomerRepository(mockConnection.Context);

    _defaultCustomer = new Customer(new Name("João", "Pedro"), new Email("joaopedro@gmail.com"), new Phone("+55 (55) 33112-3456"), "1234", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999));

    _customers.AddRange(
    [
      _defaultCustomer,
      new Customer(new Name("Maria", "Silva"), new Email("maria.silva@example.com"), new Phone("+55 (11) 98765-4321"), "senha123", EGender.Feminine, DateTime.Now.AddYears(-25), new Address("Brazil", "Rio de Janeiro", "Rua Copacabana", 123)),
      new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 12345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)),
      new Customer(new Name("Ana", "Santos"), new Email("anasantos@example.com"), new Phone("+55 (31) 98765-4321"), "ana456", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Belo Horizonte", "Avenida Afonso Pena", 456)),
      new Customer(new Name("Rafael", "Silveira"), new Email("rafaelsilveira@example.com"), new Phone("+55 (19) 98765-4321"), "rafa789", EGender.Masculine, DateTime.Now.AddYears(-32), new Address("Brazil", "Campinas", "Rua Barão de Jaguara", 789))
    ]);

    await mockConnection.Context.Customers.AddRangeAsync(_customers);
    await mockConnection.Context.SaveChangesAsync();
  }

  [TestCleanup]
  public async Task CleanupTest()
  {
    mockConnection.Context.Customers.RemoveRange(_customers);
    await mockConnection.Context.SaveChangesAsync();
    _customers.Clear();
  }

  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var customer = await _customerRepository.GetByIdAsync(_defaultCustomer.Id);

    Assert.IsNotNull(customer);
    Assert.AreEqual(_defaultCustomer.Name.FirstName, customer.FirstName);
    Assert.AreEqual(_defaultCustomer.Name.LastName, customer.LastName);
    Assert.AreEqual(_defaultCustomer.Email.Address, customer.Email);
    Assert.AreEqual(_defaultCustomer.Phone.Number, customer.Phone);
    Assert.AreEqual(_defaultCustomer.Id, customer.Id);
  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new UserQueryParameters(0, 1, _defaultCustomer.Name.FirstName, null, null, null, null, null, null, null);
    var customers = await _customerRepository.GetAsync(parameters);

    var customer = customers.ToList()[0];

    Assert.IsNotNull(customer);
    Assert.AreEqual(_defaultCustomer.Name.FirstName, customer.FirstName);
    Assert.AreEqual(_defaultCustomer.Name.LastName, customer.LastName);
    Assert.AreEqual(_defaultCustomer.Email.Address, customer.Email);
    Assert.AreEqual(_defaultCustomer.Phone.Number, customer.Phone);
    Assert.AreEqual(_defaultCustomer.Id, customer.Id);
  }

  [TestMethod]
  public async Task GetAsync_WhereNameIncludesJoao_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 1, "João", null, null, null, null, null, null, null);
    var customers = await _customerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());
    foreach (var customer in customers)
    {
      Assert.IsNotNull(customer);
      Assert.IsTrue(customer.FirstName.Contains("João"));
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereEmailContainsCom_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, null, ".com", null, null, null, null, null, null);
    var customers = await _customerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());
    foreach (var customer in customers)
    {
      Assert.IsNotNull(customer);
      Assert.IsTrue(customer.Email.Contains(".com"));
    }
  }

  [TestMethod]
  public async Task GetAsync_WherePhoneContains55_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, null, null, "55", null, null, null, null, null);
    var customers = await _customerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());
    foreach (var customer in customers)
    {
      Assert.IsNotNull(customer);
      Assert.IsTrue(customer.Phone.Contains("55"));
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereGenderEqualsMasculine_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, null, null, null, EGender.Masculine, null, null, null, null);
    var customers = await _customerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());
    foreach (var customer in customers)
    {
      Assert.IsNotNull(customer);
      Assert.AreEqual(EGender.Masculine, customer.Gender);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereDateOfBirthGratherThan2000_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, null, null, null, null, DateTime.Now.AddYears(-24), "gt", null, null);
    var customers = await _customerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());
    foreach (var customer in customers)
    {
      Assert.IsNotNull(customer);
      Assert.IsTrue(DateTime.Now.AddYears(-24) < customer.DateOfBirth);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtGratherThanYesterday_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, null, null, null, null, null, null, DateTime.Now.AddDays(-1), "gt");
    var customers = await _customerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());
    foreach (var customer in customers)
    {
      Assert.IsNotNull(customer);
      Assert.IsTrue(DateTime.Now.AddDays(-1) < customer.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtLessThanToday_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, null, null, null, null, null, null, DateTime.Now.AddDays(1), "lt");
    var customers = await _customerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());
    foreach (var customer in customers)
    {
      Assert.IsNotNull(customer);
      Assert.IsTrue(DateTime.Now.AddDays(1) > customer.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtEquals_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, null, null, null, null, null, null, _defaultCustomer.CreatedAt, "eq");
    var customers = await _customerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());
    foreach (var customer in customers)
    {
      Assert.IsNotNull(customer);
      Assert.AreEqual(_defaultCustomer.CreatedAt, customer.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereEmailIncludesExample_And_PhoneIncludes55_And_GenderEqualsMasculine_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, null, "example", "55", EGender.Masculine, null, null, null, null);
    var customers = await _customerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());

    foreach (var customer in customers)
    {
      Assert.IsNotNull(customer);
      Assert.IsTrue(customer.Email.Contains("example"));
      Assert.IsTrue(customer.Phone.Contains("55"));
      Assert.AreEqual(EGender.Masculine, customer.Gender);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereNameIncludesR_And_DateOfBirthLessThan31Years_And_CreatedAtLessThanTomorrow_ReturnsCustomers()
  {
    var parameters = new UserQueryParameters(0, 100, "R", null, null, null, DateTime.Now.AddYears(-31), "lt", DateTime.Now.AddDays(1), "lt");

    var customers = await _customerRepository.GetAsync(parameters);

    Assert.IsTrue(customers.Any());

    foreach (var customer in customers)
    {
      Assert.IsNotNull(customer);
      Assert.IsTrue(customer.FirstName.Contains('R'));
      Assert.IsTrue(DateTime.Now.AddYears(-31) > customer.DateOfBirth);
      Assert.IsTrue(DateTime.Now.AddDays(1) > customer.CreatedAt);
    }
  }
}
