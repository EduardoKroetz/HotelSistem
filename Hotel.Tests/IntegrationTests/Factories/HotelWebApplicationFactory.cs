using Hotel.Domain.Services.TokenServices;
using Hotel.Tests.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.IntegrationTests.Factories;

public class HotelWebApplicationFactory : WebApplicationFactory<Startup>
{
  public DbFixture DbFixture { get; private set; } = null!;

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    base.ConfigureWebHost(builder);

    builder.UseEnvironment("IntegrationTests");

    builder.ConfigureServices(services =>
    {
      DbFixture = new DbFixture(services);
    });
  }

  public async Task<string> LoginFullAccess()
  {
    var admin = await DbFixture.DbContext.Admins.FirstOrDefaultAsync(x => x.Email.Address == "leonardoDiCaprio199@gmail.com");

    var token = new TokenService().GenerateToken(admin!);
    return token;
  }

}
