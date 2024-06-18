using Hotel.Domain.Data;
using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.EmployeeEntity;
using Hotel.Domain.Services.TokenServices;
using Hotel.Tests.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

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

    public async Task<string> LoginCustomer()
    {
        var dbContext = Services.GetRequiredService<HotelDbContext>();
        var customerId = await DbFixture.InsertCustomer();
        var customer = await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == customerId);
        var token = _tokenService.GenerateToken(customer!);
        return token;
    }

    public void Login(HttpClient client, Admin admin)
    {
        var token = _tokenService.GenerateToken(admin);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    public void Login(HttpClient client, Customer customer)
    {
        var token = _tokenService.GenerateToken(customer);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    public void Login(HttpClient client, Employee employee)
    {
        var token = _tokenService.GenerateToken(employee);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public void Login(HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

}
