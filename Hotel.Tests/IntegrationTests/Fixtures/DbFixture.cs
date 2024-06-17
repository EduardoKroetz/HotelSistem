using Hotel.Domain.Data;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.IntegrationTests.Fixtures;

public class DbFixture : IDisposable
{
  public HotelDbContext DbContext;
  private SqliteConnection _connection;

  public DbFixture()
  {
    _connection = new SqliteConnection("DataSource=:memory:");
    _connection.Open();

    var options = new DbContextOptionsBuilder<HotelDbContext>()
      .UseSqlite(_connection)
      .Options;

    DbContext = new HotelDbContext(options);
    DbContext.Database.EnsureCreated();

    Task.WhenAny(SeedDatabase());
  }

  public void Dispose()
  {
    DbContext.Database.EnsureDeleted();
    DbContext.Dispose();
    _connection.Dispose();
  }

  public async Task InsertRootAdmin()
  {
    var hashedPassword = PasswordHasher.HashPassword("123");
    var insertRootAdmin = $"INSERT INTO [Admins]([Id],[FirstName],[LastName],[Email],[Phone],[PasswordHash],[IsRootAdmin],[IncompleteProfile],[CreatedAt]) VALUES ('b69f06d9-0dda-40f5-8fcb-797029d36e26','Leonardo','Dicaprio','leonardoDiCaprio199@gmail.com','+55 (11) 19391-1319','{hashedPassword}',1,1,'{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}')";
    await DbContext.Database.ExecuteSqlRawAsync(insertRootAdmin);
  }

  public async Task<Guid> InsertCustomer()
  {
    var customer = new Customer(
      new Name("Gustavo", "Souza"),
      new Email("gustavoSouza@gmail.com"),
      new Phone("+55 (27) 93456-7890"),
      "123",
      EGender.Masculine,
      DateTime.Now.AddYears(-33),
      new Address("Brazil", "Vitória", "ES-1010", 1010)
    );
    await DbContext.Customers.AddAsync(customer);
    await DbContext.SaveChangesAsync();

    return customer.Id;
  }

  public async Task SeedDatabase()
  {
    await new SeedPermissions(DbContext).CreatePermissionsAsync();
  }
}
