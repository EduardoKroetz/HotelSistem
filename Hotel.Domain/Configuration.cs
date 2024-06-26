namespace Hotel.Domain;

public static class Configuration
{
    public static string ConnectionString = null!;
    public static string JwtKey = null!;
    public static string EmailToSendEmail = null!;
    public static string PasswordToSendEmail = null!;
    public static StripeModel Stripe = new();
    public static string BaseUrl = "https://localhost:7100";
}

public class StripeModel 
{
    public string SecretKey { get; set; } = null!;
    public string PublishableKey { get; set; } = null!;
};