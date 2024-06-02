using Hotel.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Services.VerificationServices;

public partial class VerificationService
{
  private readonly Timer _timer;
  private readonly DbContextOptions<HotelDbContext> _dbContextOptions;

  public VerificationService(DbContextOptions<HotelDbContext> options)
  {
    _dbContextOptions = options;
    _timer = new Timer(DeleteExpiredCodes,null, TimeSpan.Zero, TimeSpan.FromMinutes(20));
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
      }
    }
  }


}
