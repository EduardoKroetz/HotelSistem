using Hotel.Domain.Data;
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
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Initialization;

public static class ConfigureDependencies
{
  public static void Configure(WebApplicationBuilder builder)
  {
    builder.Services.AddDbContext<HotelDbContext>(opt =>
    {
      opt.UseSqlServer(Configuration.Configuration.ConnectionString);
    });

    //Configurar repositórios
    builder.Services.AddScoped<IAdminRepository, AdminRepository>();
    builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
    builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    builder.Services.AddScoped<IResponsabilityRepository, ResponsabilityRepository>();
    builder.Services.AddScoped<IRoomInvoiceRepository, RoomInvoiceRepository>();
    builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
    builder.Services.AddScoped<IRoomRepository, RoomRepository>();
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<IReportRepository, ReportRepository>();
    builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

    //Configurar handlers
    builder.Services.AddScoped<AdminHandler>();
    builder.Services.AddScoped<PermissionHandler>();
    builder.Services.AddScoped<CustomerHandler>();
    builder.Services.AddScoped<FeedbackHandler>();
    builder.Services.AddScoped<EmployeeHandler>();
    builder.Services.AddScoped<ResponsabilityHandler>();
    builder.Services.AddScoped<RoomInvoiceHandler>();
    builder.Services.AddScoped<ReservationHandler>();
    builder.Services.AddScoped<RoomHandler>();
    builder.Services.AddScoped<CategoryHandler>();
    builder.Services.AddScoped<ReportHandler>();
    builder.Services.AddScoped<ServiceHandler>();
    builder.Services.AddScoped<LoginHandler>();


  }
}
