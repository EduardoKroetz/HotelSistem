using Hotel.Domain.Entities.Base;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IRepository<T> where T : Entity
{
  public Task CreateAsync(T model);
  public Task<IEnumerable<T>> GetEntitiesAsync();
  public Task<T?> GetEntityByIdAsync(Guid id);
  public void Update(T model);
  public void Delete(T model);
  public void Delete(Guid id);
  public Task SaveChangesAsync();
}