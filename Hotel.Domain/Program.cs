using Hotel.Domain.Initialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Console.OutputEncoding = Encoding.UTF8;

builder.Configuration.AddEnvironmentVariables();

LoadConfigurationClass.Configure(builder);
ConfigureServices.Configure(builder);
ConfigureAuthentication.Configure(builder.Services);
ConfigureCors.Configure(builder.Services);

//builder.WebHost.UseUrls("http://0.0.0.0:80");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    app.UseHttpsRedirection();

app.UseRouting();
app.UseExceptionHandler();
app.UseStaticFiles();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Startup { }