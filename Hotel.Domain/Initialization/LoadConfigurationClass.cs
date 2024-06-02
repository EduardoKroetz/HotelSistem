
namespace Hotel.Domain.Initialization;

public static class LoadConfigurationClass
{
  public static void Configure(WebApplicationBuilder builder)
  {
    Configuration.Configuration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
    Configuration.Configuration.JwtKey = builder.Configuration.GetValue<string>("JwtKey") ?? null!;
    Configuration.Configuration.EmailToSendEmail = builder.Configuration.GetValue<string>("EmailToSendEmail") ?? "";
    Configuration.Configuration.PasswordToSendEmail = builder.Configuration.GetValue<string>("PasswordToSendEmail") ?? "";
  }

}
