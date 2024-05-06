using Hotel.Domain.Configuration;
using Hotel.Domain.Data;
using Hotel.Domain.Extensions;
using Hotel.Domain.Handlers.AdminContext.AdminHandlers;
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
  builder.Services.AddScoped<AdminHandler>();
}