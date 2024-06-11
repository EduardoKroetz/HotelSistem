using Hotel.Domain.Data;
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
    var insertRootAdmin = $"INSERT INTO [Admins]([Id],[FirstName],[LastName],[Email],[Phone],[PasswordHash],[IsRootAdmin],[IncompleteProfile],[CreatedAt]) VALUES ('b69f06d9-0dda-40f5-8fcb-797029d36e26','Leonardo','Dicaprio','leonardoDiCaprio199@gmail.com','+55 (11) 99391-0312','{hashedPassword}',1,1,'{DateTime.Now}')";
    await DbContext.Database.ExecuteSqlRawAsync(insertRootAdmin);
    await DbContext.SaveChangesAsync();
  }
}
