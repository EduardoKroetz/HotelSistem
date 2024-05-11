using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories;
using Hotel.Domain.ValueObjects;

namespace Hotel.Tests.Repositories.CustomerContext;

[TestClass]
public class CustomerRepositoryTest : GenericRepositoryTest<Customer,CustomerRepository>
{
  private static ConfigMockConnection _mockConnection = null!;
  private static CustomerRepository _customerRepository = null!;
  private static readonly Customer _customerToBeCreated = new(new Name("Vinicius", "Santos"), new Email("viniciuos@gmail.com"), new Phone("+55 (55) 33112-3456"), "1234", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999));

  private static readonly List<Customer> _customers =
  [
    new Customer(new Name("João", "Pedro"), new Email("joaopedro@gmail.com"), new Phone("+55 (55) 33112-3456"), "1234", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999)),
    new Customer(new Name("João", "Pedro"), new Email("joaopedro@gmail.com"), new Phone("+55 (55) 33112-3456"), "1234", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999)),
    new Customer(new Name("Maria", "Silva"), new Email("maria.silva@example.com"), new Phone("+55 (11) 98765-4321"), "senha123", EGender.Feminine, DateTime.Now.AddYears(-25), new Address("Brazil", "Rio de Janeiro", "Rua Copacabana", 123)),
    new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 12345-6789"), "_admin123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)),
    new Customer(new Name("Ana", "Santos"), new Email("anasantos@example.com"), new Phone("+55 (31) 98765-4321"), "ana456", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Belo Horizonte", "Avenida Afonso Pena", 456)),
    new Customer(new Name("Rafael", "Silveira"), new Email("rafaelsilveira@example.com"), new Phone("+55 (19) 98765-4321"), "rafa789", EGender.Masculine, DateTime.Now.AddYears(-32), new Address("Brazil", "Campinas", "Rua Barão de Jaguara", 789))
  ];

  public CustomerRepositoryTest() : base( _customerRepository,_customers, _customerToBeCreated) {}

  [ClassInitialize]
  public static async Task Setup(TestContext context)
  {
    var mockConnection = new ConfigMockConnection();
    await mockConnection.Initialize();
    _mockConnection = mockConnection;

    _customerRepository = new CustomerRepository(_mockConnection.Context);
  }

  [ClassCleanup]
  public static void Cleanup()
  => _mockConnection.Dispose();

  [TestInitialize]
  public async Task SetupTest()
  {
    _customers.AddRange(
      [
        new Customer(new Name("João", "Pedro"), new Email("joaopedro@gmail.com"), new Phone("+55 (55) 33112-3456"), "1234", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999)),
        new Customer(new Name("João", "Pedro"), new Email("joaopedro@gmail.com"), new Phone("+55 (55) 33112-3456"), "1234", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999)),
        new Customer(new Name("Maria", "Silva"), new Email("maria.silva@example.com"), new Phone("+55 (11) 98765-4321"), "senha123", EGender.Feminine, DateTime.Now.AddYears(-25), new Address("Brazil", "Rio de Janeiro", "Rua Copacabana", 123)),
        new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 12345-6789"), "_admin123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)),
        new Customer(new Name("Ana", "Santos"), new Email("anasantos@example.com"), new Phone("+55 (31) 98765-4321"), "ana456", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Belo Horizonte", "Avenida Afonso Pena", 456)),
        new Customer(new Name("Rafael", "Silveira"), new Email("rafaelsilveira@example.com"), new Phone("+55 (19) 98765-4321"), "rafa789", EGender.Masculine, DateTime.Now.AddYears(-32), new Address("Brazil", "Campinas", "Rua Barão de Jaguara", 789))
      ]
    );


    _mockConnection.Context.Customers.AddRange(_customers);
    await _mockConnection.Context.SaveChangesAsync();
  }

  [TestCleanup]
  public void CleanupTest()
  => _customers.Clear();

  [TestMethod]
  public async Task GetByIdAsync_ValidId_ReturnsAdmin()
  {
    var customer = await _customerRepository.GetByIdAsync(_customers[0].Id);

    Assert.IsNotNull(customer);
    Assert.AreEqual("João", customer.FirstName);
  }

  [TestMethod]
  public async Task GetAsync_ReturnsPermissions()
  {
    var customers = await _customerRepository.GetAsync();

    Assert.IsNotNull(customers);
    Assert.IsTrue(customers.Any());
  }
}