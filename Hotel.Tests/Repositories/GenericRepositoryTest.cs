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

  public async static Task<ConfigMockConnection> InitializeMockConnection()
  {
    var mockConnection = new ConfigMockConnection();
    await mockConnection.Initialize();
    return mockConnection;
  }

  [TestMethod]
  public async Task CreateAsync_MustCreate()
  {
    await _repository.CreateAsync(_entityToBeCreated);
    await _repository.SaveChangesAsync();

    var createdEntity = await _repository.GetEntityByIdAsync(_entityToBeCreated.Id);

    Assert.IsNotNull(createdEntity);
    Assert.AreEqual(_entityToBeCreated.Id, createdEntity?.Id);
  }

  [TestMethod]
  public async Task Delete_MustDelete()
  {
    var entityToBeDeleted = _entities[1];
    _repository.Delete(entityToBeDeleted);
    await _repository.SaveChangesAsync();

    var deletedEntity = await _repository.GetEntityByIdAsync(entityToBeDeleted.Id);
    Assert.IsNull(deletedEntity);
  }

  [TestMethod]
  public async Task DeleteById_MustDelete()
  {
    var entityToBeDeleted = _entities[1];
    _repository.Delete(entityToBeDeleted.Id);
    await _repository.SaveChangesAsync();

    var deletedEntity = await _repository.GetEntityByIdAsync(entityToBeDeleted.Id);
    Assert.IsNull(deletedEntity);
  }

  [TestMethod]
  [ExpectedException(typeof(ArgumentException))]
  public async Task DeleteNonExistEntityById_ExpectedException()
  {
    _repository.Delete(Guid.NewGuid());
    await _repository.SaveChangesAsync();
  }

  [TestMethod]
  public async Task GetEntityByIdAsync_ReturnsEntity()
  {
    var entity = await _repository.GetEntityByIdAsync(_entities[1].Id);
    Assert.IsNotNull(entity);
    Assert.AreEqual(_entities[1].Id, entity.Id);
  }

  [TestMethod]
  public async Task GetEntitiesAsync_ReturnsEntities()
  {
    var entities = await _repository.GetEntitiesAsync();

    Assert.IsNotNull(entities);
    Assert.IsTrue(entities.Any());
  }

  [TestMethod]
  public async Task UpdateEntity_MustApplyEntityChanges()
  {
    var entity = await _repository.GetEntityByIdAsync(_entities[1].Id);
    var date = DateTime.Now.Date.AddDays(1);
    entity?.ChangeCreatedAt(date);

    await _repository.SaveChangesAsync();

    var updatedEntity = await _repository.GetEntityByIdAsync(_entities[1].Id);

    Assert.IsNotNull(updatedEntity);
    Assert.AreEqual(date,updatedEntity.CreatedAt);
  }

}

