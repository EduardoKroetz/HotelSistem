using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Hotel.Domain.Initialization;

public static class ConfigureAuthentication
{
    public static void Configure(IServiceCollection services)
    {
        var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true; // Garantir HTTPS
            options.SaveToken = true; // Salvar o token no contexto de autenticação

            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true, // Validar a chave de assinatura
                IssuerSigningKey = new SymmetricSecurityKey(key), // Chave de assinatura simétrica
                ValidateAudience = false,
                ValidateIssuer = false
            };
        });
    }
}