namespace Hotel.Domain.Initialization;

public static class ConfigureCors
{
    public static void Configure(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins("http://127.0.0.1:5500")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
}
