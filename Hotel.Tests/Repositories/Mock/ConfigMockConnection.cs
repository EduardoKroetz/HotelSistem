using Hotel.Domain.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.Mock;
public class ConfigMockConnection
{
  public SqliteConnection Connection { get; private set; }
  public HotelDbContext Context { get; private set; }

  public ConfigMockConnection()
  {
    Connection = new SqliteConnection("DataSource=:memory:");
    Connection.Open();

    var options = new DbContextOptionsBuilder<HotelDbContext>()
        .UseSqlite(Connection)
        .Options;

    Context = new HotelDbContext(options);
  }

  public async Task Initialize()
  => await Context.Database.EnsureCreatedAsync();

  public void Dispose()
  => Connection.Dispose();
}

