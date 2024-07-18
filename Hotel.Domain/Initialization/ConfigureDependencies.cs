using Hotel.Domain.Data;
using Hotel.Domain.Handlers.AdminHandlers;
using Hotel.Domain.Handlers.PermissionHandlers;
using Hotel.Domain.Handlers.CustomerHandlers;
using Hotel.Domain.Handlers.FeedbackHandlers;
using Hotel.Domain.Handlers.EmployeeHandlers;
using Hotel.Domain.Handlers.y.ResponsibilityHandlers;
using Hotel.Domain.Handlers.LoginHandlers;
using Hotel.Domain.Handlers.InvoiceHandlers;
using Hotel.Domain.Handlers.ReservationHandlers;
using Hotel.Domain.Handlers.CategoryHandlers;
using Hotel.Domain.Handlers.ReportHandlers;
using Hotel.Domain.Handlers.RoomHandlers;
using Hotel.Domain.Handlers.ServiceHandler;
using Hotel.Domain.Handlers.VerificationHandlers;
using Hotel.Domain.Repositories;
using Hotel.Domain.Repositories.Interfaces;
using Hotel.Domain.Services;
using Hotel.Domain.Services.EmailServices;
using Hotel.Domain.Services.EmailServices.Interfaces;
using Hotel.Domain.Services.LoginServices;
using Hotel.Domain.Services.TokenServices;
using Hotel.Domain.Services.UserServices;
using Hotel.Domain.Services.UserServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using Hotel.Domain.Services.Interfaces;
using Hotel.Domain.Middlewares;
using Serilog;
using Hotel.Domain.Services.CleanupExpiredCodesServices;

namespace Hotel.Domain.Initialization;

public static class ConfigureServices
{
    public static void Configure(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        //ExceptionHandler
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        //Logs
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        services.AddLogging(logginBuilder =>
        {
            logginBuilder.ClearProviders();
            logginBuilder.AddSerilog();
        });

        if (builder.Environment.IsDevelopment() || builder.Environment.IsProduction())
        {
            var options = new DbContextOptionsBuilder<HotelDbContext>()
              .UseSqlServer(Configuration.ConnectionString)
              .Options;

            services.AddSingleton(options);

            services.AddDbContext<HotelDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.ConnectionString);
            });
        }

        //Configurar repositórios
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IResponsibilityRepository, ResponsibilityRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IVerificationCodeRepository, VerificationCodeRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<IDeslikeRepository, DeslikeRepository>();


        //Services
        services.AddScoped<IStripeService, StripeService>();
        services.AddSingleton<TokenService>();
        services.AddSingleton<LoginService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IUserService, UserService>();

        //Background Services
        services.AddHostedService<CleanupExpiredCodesService>();

        //Configurar handlers
        services.AddScoped<AdminHandler>();
        services.AddScoped<PermissionHandler>();
        services.AddScoped<CustomerHandler>();
        services.AddScoped<FeedbackHandler>();
        services.AddScoped<EmployeeHandler>();
        services.AddScoped<ResponsibilityHandler>();
        services.AddScoped<InvoiceHandler>();
        services.AddScoped<ReservationHandler>();
        services.AddScoped<RoomHandler>();
        services.AddScoped<CategoryHandler>();
        services.AddScoped<ReportHandler>();
        services.AddScoped<ServiceHandler>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<VerificationHandler>();

    }
}
