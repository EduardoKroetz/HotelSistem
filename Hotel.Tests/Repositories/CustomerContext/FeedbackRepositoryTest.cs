
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.Entities;

namespace Hotel.Tests.Repositories.FeedbackContext;

[TestClass]
public class FeedbackRepositoryTest : GenericRepositoryTest<Feedback, FeedbackRepository>
{
  private static ConfigMockConnection _mockConnection = null!;
  private static FeedbackRepository _feedbackRepository = null!;
  private static readonly Feedback _feedbackToBeCreated = new("Legal", 7, TestParameters.Customer.Id, TestParameters.Reservation.Id, TestParameters.Room.Id);
  private static readonly List<Feedback> _feedbacks = [];

  public FeedbackRepositoryTest() : base(_feedbackRepository, _feedbacks, _feedbackToBeCreated) { }

  [ClassInitialize]
  public static async Task Setup(TestContext context)
  {
    _mockConnection = await InitializeMockConnection();

    await new CategoryRepository(_mockConnection.Context).CreateAsync(TestParameters.Category);
    await new RoomRepository(_mockConnection.Context).CreateAsync(TestParameters.Room);
    await new CustomerRepository(_mockConnection.Context).CreateAsync(TestParameters.Customer);
    await new ReservationRepository(_mockConnection.Context).CreateAsync(TestParameters.Reservation);

    await _mockConnection.Context.SaveChangesAsync();

    _feedbackRepository = new FeedbackRepository(_mockConnection.Context);
  }

  [ClassCleanup]
  public static void Cleanup()
  => _mockConnection.Dispose();

  [TestInitialize]
  public async Task SetupTest()
  {
    _feedbacks.AddRange(
      [
        new("Legal", 7, TestParameters.Customer.Id, TestParameters.Reservation.Id, TestParameters.Room.Id),
        new("Gostei", 8, TestParameters.Customer.Id, TestParameters.Reservation.Id, TestParameters.Room.Id),
        new("O quarto estava em pessimas condições", 7, TestParameters.Customer.Id, TestParameters.Reservation.Id, TestParameters.Room.Id),
        new("O serviço é ótimo", 9, TestParameters.Customer.Id, TestParameters.Reservation.Id, TestParameters.Room.Id),
        new("O serviço é horrível", 1, TestParameters.Customer.Id, TestParameters.Reservation.Id, TestParameters.Room.Id),
      ]
    );


    _mockConnection.Context.Feedbacks.AddRange(_feedbacks);

    await _mockConnection.Context.SaveChangesAsync();
  }

  [TestCleanup]
  public void CleanupTest()
  => _feedbacks.Clear();

  [TestMethod]
  public async Task GetByIdAsync_ValidId_ReturnsAdmin()
  {
    var feedback = await _feedbackRepository.GetByIdAsync(_feedbacks[0].Id);

    Assert.IsNotNull(feedback);
    Assert.AreEqual("Legal", feedback.Comment);
  }

  [TestMethod]
  public async Task GetAsync_ReturnsPermissions()
  {
    var feedbacks = await _feedbackRepository.GetAsync();

    Assert.IsNotNull(feedbacks);
    Assert.IsTrue(feedbacks.Any());
  }
}