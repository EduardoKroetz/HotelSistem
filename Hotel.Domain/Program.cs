using Hotel.Domain.Data;
using Hotel.Domain.Extensions;
using Hotel.Domain.Initialization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

LoadConfigurationClass.Configure(builder);
ConfigureDependencies.Configure(builder);
ConfigureAuthentication.Configure(builder.Services);

//builder.WebHost.UseUrls("http://0.0.0.0:80");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
  app.UseHttpsRedirection();

app.UseRouting();
app.UseHandleExceptions();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Startup {}