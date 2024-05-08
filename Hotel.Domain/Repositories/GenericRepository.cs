using Hotel.Domain.Data;
using Hotel.Domain.Entities.Base;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
  protected readonly HotelDbContext _context;

  public GenericRepository(HotelDbContext context)
  => _context = context;

  public async Task CreateAsync(TEntity model)
  => await _context.Set<TEntity>().AddAsync(model);

  public void Delete(TEntity model)
  => _context.Set<TEntity>().Remove(model);

  public void Delete(Guid id)
  {
    var model = _context.Set<TEntity>().FirstOrDefault(x => x.Id == id);
    if (model == null)
      throw new ArgumentException($"Não foi possível deletar o item pois ele não existe.");
    _context.Set<TEntity>().Remove(model);
  }
  
  public async Task<TEntity?> GetEntityByIdAsync(Guid id)
  {
    return await _context
      .Set<TEntity>()
      .FirstOrDefaultAsync(x => x.Id == id);
  }

  public async Task<IEnumerable<TEntity>> GetEntitiesAsync()
  {
    return await _context
      .Set<TEntity>()
      .AsNoTracking()
      .ToListAsync();
  }

  public void Update(TEntity model)
  => _context.Set<TEntity>().Update(model);
  
  public Task SaveChangesAsync()
  => _context.SaveChangesAsync();

}