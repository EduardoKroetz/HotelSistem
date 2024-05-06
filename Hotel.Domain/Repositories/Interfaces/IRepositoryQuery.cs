namespace Hotel.Domain.Repositories.Interfaces;

public interface IRepositoryQuery<TQuery>
{
  public Task<TQuery?> GetByIdAsync(Guid id);
  public Task<IEnumerable<TQuery>> GetAsync();
}