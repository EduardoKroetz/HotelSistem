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
using Hotel.Domain.Services.Authorization;
using Hotel.Domain.Services.EmailServices;
using Hotel.Domain.Services.EmailServices.Interfaces;
using Hotel.Domain.Services.LoginServices;
using Hotel.Domain.Services.TokenServices;
using Hotel.Domain.Services.UserServices;
using Hotel.Domain.Services.UserServices.Interfaces;
using Hotel.Domain.Services.VerificationServices;
using Microsoft.EntityFrameworkCore;
using Hotel.Domain.Services.Interfaces;
using Hotel.Domain.Middlewares;

namespace Hotel.Domain.Initialization;

public static class ConfigureDependencies
{
    public static void Configure(WebApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        if (builder.Environment.IsDevelopment() || builder.Environment.IsProduction())
        {
            var options = new DbContextOptionsBuilder<HotelDbContext>()
              .UseSqlServer(Configuration.ConnectionString)
              .Options;

            builder.Services.AddSingleton(options);

            builder.Services.AddDbContext<HotelDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.ConnectionString);
            });
        }

        //Configurar repositórios
        builder.Services.AddScoped<IAdminRepository, AdminRepository>();
        builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IResponsibilityRepository, ResponsibilityRepository>();
        builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
        builder.Services.AddScoped<IRoomRepository, RoomRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IReportRepository, ReportRepository>();
        builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
        builder.Services.AddScoped<IVerificationCodeRepository, VerificationCodeRepository>();
        builder.Services.AddScoped<ILikeRepository, LikeRepository>();
        builder.Services.AddScoped<IDeslikeRepository, DeslikeRepository>();


        //Services
        builder.Services.AddSingleton<AuthorizationService>();
        builder.Services.AddScoped<IStripeService, StripeService>();
        builder.Services.AddSingleton<TokenService>();
        builder.Services.AddSingleton<LoginService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddSingleton<VerificationService>();

        //Configurar handlers
        builder.Services.AddScoped<AdminHandler>();
        builder.Services.AddScoped<PermissionHandler>();
        builder.Services.AddScoped<CustomerHandler>();
        builder.Services.AddScoped<FeedbackHandler>();
        builder.Services.AddScoped<EmployeeHandler>();
        builder.Services.AddScoped<ResponsibilityHandler>();
        builder.Services.AddScoped<InvoiceHandler>();
        builder.Services.AddScoped<ReservationHandler>();
        builder.Services.AddScoped<RoomHandler>();
        builder.Services.AddScoped<CategoryHandler>();
        builder.Services.AddScoped<ReportHandler>();
        builder.Services.AddScoped<ServiceHandler>();
        builder.Services.AddScoped<LoginHandler>();
        builder.Services.AddScoped<VerificationHandler>();

    }
}
