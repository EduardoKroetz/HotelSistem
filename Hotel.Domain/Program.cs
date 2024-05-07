using Hotel.Domain.Configuration;
using Hotel.Domain.Data;
using Hotel.Domain.Extensions;
using Hotel.Domain.Handlers.AdminContext.AdminHandlers;
using Hotel.Domain.Handlers.AdminContext.PermissionHandlers;
using Hotel.Domain.Handlers.CustomerContext.CustomerHandlers;
using Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;
using Hotel.Domain.Handlers.EmployeeContexty.EmployeeHandlers;
using Hotel.Domain.Handlers.EmployeeContexty.ResponsabilityHandlers;
using Hotel.Domain.Repositories;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

LoadConfiguration(builder);
ConfigureDependencies(builder);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHandleExceptions();
app.MapControllers();


app.Run();


void LoadConfiguration(WebApplicationBuilder builder)
{
  Configuration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
}

void ConfigureDependencies(WebApplicationBuilder builder)
{ 
  builder.Services.AddDbContext<HotelDbContext>(opt =>
  {
    opt.UseSqlServer(Configuration.ConnectionString);
  });
  builder.Services.AddScoped<IAdminRepository ,AdminRepository>();
  builder.Services.AddScoped<IPermissionRepository ,PermisisionRepository>();
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
}