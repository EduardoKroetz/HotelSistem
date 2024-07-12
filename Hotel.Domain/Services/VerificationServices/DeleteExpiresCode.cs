using Hotel.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Services.VerificationServices;

public partial class VerificationService
{
    private readonly Timer _timer;
    private readonly DbContextOptions<HotelDbContext> _dbContextOptions;
    private readonly ILogger<VerificationService> _logger;

    public VerificationService(Timer timer, DbContextOptions<HotelDbContext> dbContextOptions, ILogger<VerificationService> logger)
    {
        _timer = timer;
        _dbContextOptions = dbContextOptions;
        _logger = logger;
    }

    private async void DeleteExpiredCodes(object? state)
    {
        using (var context = new HotelDbContext(_dbContextOptions))
        {
            var expirationTime = DateTime.Now.AddMinutes(-20);
            var expiredCodes = await context.VerificationCodes
              .Where(x => x.CreatedAt < expirationTime)
            .ToListAsync();

            if (expiredCodes.Count > 0)
            {
                context.VerificationCodes.RemoveRange(expiredCodes);
                await context.SaveChangesAsync();
                _logger.LogInformation($"Foram excluidos {expiredCodes.Count} códigos de verificação expirados");
            }
        }
    }


}
