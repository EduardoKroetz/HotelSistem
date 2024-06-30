using Hotel.Domain.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.UnitTests.Repositories.InMemoryDatabase;
public class SqliteDatabase
{
    public SqliteConnection Connection { get; private set; }
    public HotelDbContext Context { get; private set; }

    public SqliteDatabase()
    {
        Connection = new SqliteConnection("DataSource=:memory:");
        Connection.Open();

        var options = new DbContextOptionsBuilder<HotelDbContext>()
            .UseSqlite(Connection)
            .Options;

        Context = new HotelDbContext(options);
        Context.Database.EnsureCreatedAsync().Wait();
    }

    public async Task Initialize()
    => await Task.Delay(500);

    public void Dispose()
    => Connection.Dispose();
}

