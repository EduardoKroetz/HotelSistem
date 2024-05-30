
namespace Hotel.Domain.Initialization;

public static class LoadConfigurationClass
{
  public static void Configure(WebApplicationBuilder builder)
  {
    Configuration.Configuration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
    Configuration.Configuration.JwtKey = builder.Configuration.GetValue<string>("JwtKey") ?? null!;
  }

}
