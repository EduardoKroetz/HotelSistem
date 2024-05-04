using Hotel.Domain.Configuration;
using Hotel.Domain.Data;
using Hotel.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

LoadConfiguration(builder);
ConfigureServices(builder);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHandleExceptions();


app.Run();


void LoadConfiguration(WebApplicationBuilder builder)
{
  Configuration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
}

void ConfigureServices(WebApplicationBuilder builder)
{ 
  builder.Services.AddDbContext<HotelDbContext>(opt =>
  {
    opt.UseSqlServer(Configuration.ConnectionString);
  });
}