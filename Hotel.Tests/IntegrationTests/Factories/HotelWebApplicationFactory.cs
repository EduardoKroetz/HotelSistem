using Hotel.Domain.Data;
using Hotel.Domain.Services.TokenServices;
using Hotel.Tests.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Tests.IntegrationTests.Factories;

public class HotelWebApplicationFactory : WebApplicationFactory<Startup>
{
  public DbFixture DbFixture { get; private set; } = null!;
  private TokenService _tokenService { get; set; }

  public HotelWebApplicationFactory()
  {
    _tokenService = new TokenService();
    DbFixture = new DbFixture();
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    base.ConfigureWebHost(builder);

    builder.UseEnvironment("IntegrationTests");

    builder.ConfigureServices(services =>
    {
      services.AddSingleton(DbFixture.DbContext);
      services.AddSingleton<TokenService>();
    });
  }

  public async Task<string> LoginFullAccess()
  {
    var dbContext = Services.GetRequiredService<HotelDbContext>();
    await DbFixture.InsertRootAdmin();
    var admin = await dbContext.Admins.FirstOrDefaultAsync(x => x.Email.Address == "leonardoDiCaprio199@gmail.com");
    var token = _tokenService.GenerateToken(admin!);
    return token;
  }  
}
