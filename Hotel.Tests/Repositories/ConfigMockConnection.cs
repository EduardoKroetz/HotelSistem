
using Hotel.Domain.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories;
public class ConfigMockConnection 
{
   
  private SqliteConnection _connection;
  public HotelDbContext Context { get; private set; }

  public ConfigMockConnection()
  {
    _connection = new SqliteConnection("DataSource=:memory:");
    _connection.Open();

    var options = new DbContextOptionsBuilder<HotelDbContext>()
        .UseSqlite(_connection)
        .Options;

    Context = new HotelDbContext(options);
  }

  public async Task Initialize()
  => await Context.Database.EnsureCreatedAsync();

  public void Dispose() {
      Context?.Dispose();
      _connection?.Dispose();
  }
    

}

