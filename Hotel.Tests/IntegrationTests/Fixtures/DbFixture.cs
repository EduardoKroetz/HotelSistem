﻿using Hotel.Domain.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Tests.IntegrationTests.Fixtures;

public class DbFixture : IDisposable
{
  private HotelDbContext _dbContext;
  private SqliteConnection _connection;

  public DbFixture(IServiceCollection services)
  {
    _connection = new SqliteConnection("DataSource=:memory:");
    _connection.Open();

    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<HotelDbContext>));
    if (descriptor != null)
      services.Remove(descriptor);

    services.AddDbContext<HotelDbContext>(opt =>
    {
      opt.UseSqlite(_connection);
    });

    var serviceProvider = services.BuildServiceProvider();

    var scope = serviceProvider.CreateScope();
    _dbContext = scope.ServiceProvider.GetRequiredService<HotelDbContext>();

    _dbContext.Database.EnsureCreated();

    InsertRootAdmin(_dbContext);
  }

  public void Dispose()
  {
    _dbContext.Database.EnsureDeleted();
    _dbContext.Dispose();
    _connection.Dispose();
  }

  public void InsertRootAdmin(HotelDbContext dbContext)
  {
    var hashedPassword = PasswordHasher.HashPassword("123");
    var insertRootAdmin = $"INSERT INTO [Admins]([Id],[FirstName],[LastName],[Email],[Phone],[PasswordHash],[IsRootAdmin],[IncompleteProfile],[CreatedAt]) VALUES ('{Guid.NewGuid()}','Leonardo','Dicaprio','leonardoDiCaprio199@gmail.com','+55 (11) 99391-0312','{hashedPassword}',1,1,'{DateTime.Now}')";
    dbContext.Database.ExecuteSqlRaw(insertRootAdmin);
  }
}
