namespace Hotel.Tests.Repositories;

abstract public class BaseRepositoryTest
{
  protected static ConfigMockConnection mockConnection = null!;

  [ClassInitialize]
  public static async Task Startup(TestContext? context)
  => mockConnection = await GenericRepositoryTest.InitializeMockConnection();

  [ClassCleanup]
  public static void Cleanup()
  => mockConnection?.Dispose();
}
