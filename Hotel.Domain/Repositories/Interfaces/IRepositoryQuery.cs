namespace Hotel.Domain.Repositories.Interfaces;

public interface IRepositoryQuery<TQuery,TEnumerableQuery>
  where TQuery : class
  where TEnumerableQuery : class
{
  public Task<TQuery?> GetByIdAsync(Guid id);
  public Task<IEnumerable<TEnumerableQuery>> GetAsync();
}
public interface IRepositoryQuery<TQuery>
  where TQuery : class
{
  public Task<TQuery?> GetByIdAsync(Guid id);
  public Task<IEnumerable<TQuery>> GetAsync();
}