namespace Hotel.Domain.Initialization;

public static class LoadConfigurationClass
{
    public static void Configure(WebApplicationBuilder builder)
    {
        Configuration.JwtKey = builder.Configuration.GetValue<string>("JwtKey")!;
        Configuration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
        Configuration.EmailToSendEmail = builder.Configuration.GetValue<string>("EmailToSendEmail")!;
        Configuration.PasswordToSendEmail = builder.Configuration.GetValue<string>("PasswordToSendEmail")!;
        
        var stripe = builder.Configuration.GetSection("Stripe")!;

        Configuration.Stripe.SecretKey = stripe.GetValue<string>("SecretKey")!;
        Configuration.Stripe.PublishableKey = stripe.GetValue<string>("PublishableKey")!;
    }

}
