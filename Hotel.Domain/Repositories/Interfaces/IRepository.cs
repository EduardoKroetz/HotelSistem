using Hotel.Domain.Entities.Base;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IRepository<T> where T : Entity
{
  public Task CreateAsync(T model);
  public Task<IEnumerable<T>> GetAsync();
  public Task<T?> GetByIdAsync(Guid id);
  public void Update(T model);
  public void Delete(T model);
  public void Delete(Guid id);
}