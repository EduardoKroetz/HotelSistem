
using Hotel.Domain.Data;
using Hotel.Domain.Entities.Base;
using Hotel.Domain.Repositories.Interfaces;


namespace Hotel.Tests.Repositories;

[TestClass]
public class GenericRepositoryTest<T,TRepository>
  where T : Entity
  where TRepository : IRepository<T>
{
  private readonly TRepository _repository;
  private readonly List<T> _entities;
  private readonly T _entityToBeCreated;
  public GenericRepositoryTest(TRepository repository, List<T> entities,T entityToBeCreated) 
  {
    _repository = repository;
    _entities = entities;
    _entityToBeCreated = entityToBeCreated;
  }


  [TestMethod]
  public async Task CreateAsync_MustCreate()
  {
    //Remover entidade para então adicionar a mesma
    await _repository.CreateAsync(_entityToBeCreated);
    await _repository.SaveChangesAsync();

    var entityCreated = await _repository.GetEntityByIdAsync(_entityToBeCreated.Id);

    Assert.IsNotNull(entityCreated);
    Assert.AreEqual(_entityToBeCreated.Id, entityCreated?.Id);
  }


}

