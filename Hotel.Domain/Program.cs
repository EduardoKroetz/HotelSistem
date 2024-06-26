using Hotel.Domain.Extensions;
using Hotel.Domain.Initialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Console.OutputEncoding = Encoding.UTF8;

builder.Configuration.AddEnvironmentVariables();

LoadConfigurationClass.Configure(builder);
ConfigureDependencies.Configure(builder);
ConfigureAuthentication.Configure(builder.Services);
ConfigureCors.Configure(builder.Services);

//builder.WebHost.UseUrls("http://0.0.0.0:80");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    app.UseHttpsRedirection();

app.UseCors();
app.UseRouting();
app.UseStaticFiles();
app.UseHandleExceptions();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Startup { }