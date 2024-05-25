using Hotel.Domain.Configuration;
using Hotel.Domain.Data;
using Hotel.Domain.Extensions;
using Hotel.Domain.Handlers.AdminContext.AdminHandlers;
using Hotel.Domain.Handlers.AdminContext.PermissionHandlers;
using Hotel.Domain.Handlers.AuthenticationContext.LoginHandlers;
using Hotel.Domain.Handlers.CustomerContext.CustomerHandlers;
using Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;
using Hotel.Domain.Handlers.EmployeeContexty.EmployeeHandlers;
using Hotel.Domain.Handlers.EmployeeContexty.ResponsabilityHandlers;
using Hotel.Domain.Handlers.PaymentContext.RoomInvoiceHandlers;
using Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;
using Hotel.Domain.Handlers.RoomContext.CategoryHandlers;
using Hotel.Domain.Handlers.RoomContext.ReportHandlers;
using Hotel.Domain.Handlers.RoomContext.RoomHandlers;
using Hotel.Domain.Handlers.RoomContext.ServiceHandler;
using Hotel.Domain.Repositories.AdminContext;
using Hotel.Domain.Repositories.CustomerContext;
using Hotel.Domain.Repositories.EmployeeContext;
using Hotel.Domain.Repositories.Interfaces.AdminContext;
using Hotel.Domain.Repositories.Interfaces.CustomerContext;
using Hotel.Domain.Repositories.Interfaces.EmployeeContext;
using Hotel.Domain.Repositories.Interfaces.PaymentContext;
using Hotel.Domain.Repositories.Interfaces.ReservationContext;
using Hotel.Domain.Repositories.Interfaces.RoomContext;
using Hotel.Domain.Repositories.PaymentContext;
using Hotel.Domain.Repositories.ReservationContext;
using Hotel.Domain.Repositories.RoomContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

LoadConfiguration(builder);
ConfigureAuthentication(builder.Services);
ConfigureDependencies(builder);

//builder.WebHost.UseUrls("http://0.0.0.0:80");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseHandleExceptions();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();


void LoadConfiguration(WebApplicationBuilder builder)
{
  Configuration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
  Configuration.JwtKey = builder.Configuration.GetValue<string>("JwtKey") ?? null!;
}

void ConfigureDependencies(WebApplicationBuilder builder)
{ 
  builder.Services.AddDbContext<HotelDbContext>(opt =>
  {
    opt.UseSqlServer(Configuration.ConnectionString);
  });
  builder.Services.AddScoped<IAdminRepository ,AdminRepository>();
  builder.Services.AddScoped<IPermissionRepository ,PermissionRepository>();
  builder.Services.AddScoped<AdminHandler>();
  builder.Services.AddScoped<PermissionHandler>();
  builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
  builder.Services.AddScoped<CustomerHandler>();
  builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
  builder.Services.AddScoped<FeedbackHandler>();
  builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
  builder.Services.AddScoped<EmployeeHandler>();
  builder.Services.AddScoped<IResponsabilityRepository, ResponsabilityRepository>();
  builder.Services.AddScoped<ResponsabilityHandler>();
  builder.Services.AddScoped<IRoomInvoiceRepository, RoomInvoiceRepository>();
  builder.Services.AddScoped<RoomInvoiceHandler>();
  builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
  builder.Services.AddScoped<ReservationHandler>();
  builder.Services.AddScoped<IRoomRepository, RoomRepository>();
  builder.Services.AddScoped<RoomHandler>();
  builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
  builder.Services.AddScoped<CategoryHandler>();
  builder.Services.AddScoped<IReportRepository, ReportRepository>();
  builder.Services.AddScoped<ReportHandler>();
  builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
  builder.Services.AddScoped<ServiceHandler>();
  builder.Services.AddScoped<LoginHandler>();
}

void ConfigureAuthentication(IServiceCollection services)
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

    options.Events = new JwtBearerEvents
    {
      OnAuthenticationFailed = context =>
      {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<WebApplication>>();
        logger.LogError(context.Exception, "Authentication failed.");

        if (context.Exception is SecurityTokenExpiredException)
        {
          logger.LogError("Token has expired.");
        }
        else if (context.Exception is SecurityTokenInvalidSignatureException)
        {
          logger.LogError("Token signature validation failed.");
        }
        else if (context.Exception is SecurityTokenInvalidIssuerException)
        {
          logger.LogError("Token issuer validation failed.");
        }
        else if (context.Exception is SecurityTokenInvalidAudienceException)
        {
          logger.LogError("Token audience validation failed.");
        }
        else
        {
          logger.LogError("Token validation failed.");
        }

        return Task.CompletedTask;
      },
      OnTokenValidated = context =>
      {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<WebApplication>>();
        logger.LogInformation("Token validated successfully for {User}", context.Principal.Identity.Name);
        return Task.CompletedTask;
      },
      OnMessageReceived = context =>
      {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<WebApplication>>();
        logger.LogInformation("Token received: {Token}", context.Token);
        return Task.CompletedTask;
      }
    };
  });
}
