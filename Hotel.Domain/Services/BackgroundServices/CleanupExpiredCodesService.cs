using Hotel.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Services.CleanupExpiredCodesServices;

public class CleanupExpiredCodesService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<CleanupExpiredCodesService> _logger;

    public CleanupExpiredCodesService(IServiceScopeFactory serviceScopeFactory, ILogger<CleanupExpiredCodesService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HotelDbContext>();

                    var expirationTime = DateTime.Now.AddMinutes(-20);
                    var expiredCodes = await dbContext.VerificationCodes
                        .Where(x => x.CreatedAt < expirationTime)
                        .ToListAsync();

                    if (expiredCodes.Any())
                    {
                        dbContext.VerificationCodes.RemoveRange(expiredCodes);
                        await dbContext.SaveChangesAsync();
                        _logger.LogInformation($"Foram excluídos {expiredCodes.Count} códigos de verificação expirados");
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(20), stoppingToken);
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Um erro ocorreu no serviço de limpeza de códigos expirados. Erro: {e.Message}");
        }

    }
}
