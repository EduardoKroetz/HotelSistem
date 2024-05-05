namespace Hotel.Domain.Repositories.Interfaces;

public interface IRepository<T>
{
  public void Create(T model);
  public T Get(T model);
  public T Get(Guid id);
  public void Update(T model);
  public void Delete(T model);
  public void Delete(Guid id);
}