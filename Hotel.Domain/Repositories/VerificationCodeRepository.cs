using Hotel.Domain.Data;
using Hotel.Domain.Entities;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class VerificationCodeRepository : IVerificationCodeRepository
{
  private readonly HotelDbContext _context;
  public VerificationCodeRepository(HotelDbContext context)
  => _context = context;

  public async Task<VerificationCode?> GetCode(VerificationCode code)
  => await _context.VerificationCodes.FirstOrDefaultAsync(x => x.Code == code.Code);

  public async Task CreateAsync(VerificationCode verificationCode)
  => await _context.VerificationCodes.AddAsync(verificationCode);

  public void Delete(VerificationCode verificationCode)
  => _context.VerificationCodes.Remove(verificationCode);

  public async Task SaveChangesAsync()
  => await _context.SaveChangesAsync();

}
