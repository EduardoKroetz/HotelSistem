using Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.CategoryEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.CustomerContext;
using Hotel.Tests.Entities;
using Microsoft.EntityFrameworkCore;


namespace Hotel.Tests.Repositories.CustomerContext;

[TestClass]
public class FeedbackRepositoryTest : BaseRepositoryTest
{
  private List<Feedback> _feedbacks { get; set; } = [];
  private Feedback _defaultFeedback { get; set; } = null!;
  private FeedbackRepository _feedbackRepository { get; set; } = null!;

  [TestInitialize]
  public async Task StartupTest()
  {
    await Startup(null);
    _feedbackRepository = new FeedbackRepository(mockConnection.Context);

    var categories = await mockConnection.Context.Categories.ToListAsync();
    var rooms = await mockConnection.Context.Rooms.ToListAsync();
    var reservations = await mockConnection.Context.Reservations.ToListAsync();
    var customers = await mockConnection.Context.Customers.ToListAsync();

    _defaultFeedback = new Feedback("O quarto 321 estava organizado, mas o serviço de quarto era péssimo.", 6, _customer.Id, _reservation.Id, _room.Id);

    _feedbacks.AddRange(
    [
      _defaultFeedback,
      new Feedback("Horrível", 1, _customer.Id, _reservation.Id, _room.Id),
      new Feedback("Legal", 7, _customer.Id, _reservation.Id, _room.Id),
      new Feedback("O serviço era bom, mas o ambiente era ruim", 1, _customer.Id, _reservation.Id, _room.Id),
      new Feedback("Melhor hotel que já me hospedei", 10, _customer.Id, _reservation.Id, _room.Id),
      new Feedback("Dá pro gasto", 6, _customer.Id, _reservation.Id, _room.Id)
    ]);

    for (int i = 0; i < 6; i++)
    {
      _defaultFeedback.AddLike();
      _defaultFeedback.AddDeslike();
    }

    await mockConnection.Context.Feedbacks.AddRangeAsync(_feedbacks);
    await mockConnection.Context.SaveChangesAsync();
  }

  [TestCleanup]
  public async Task CleanupTest()
  {
    mockConnection.Context.Feedbacks.RemoveRange(_feedbacks);
    await mockConnection.Context.SaveChangesAsync();
    _feedbacks.Clear();
  }

  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var feedback = await _feedbackRepository.GetByIdAsync(_defaultFeedback.Id);

    Assert.IsNotNull(feedback);
    Assert.AreEqual(_defaultFeedback.Id, feedback.Id);
    Assert.AreEqual(_defaultFeedback.Comment, feedback.Comment);
    Assert.AreEqual(_defaultFeedback.Rate, feedback.Rate);
    Assert.AreEqual(_defaultFeedback.Likes, feedback.Likes);
    Assert.AreEqual(_defaultFeedback.Deslikes, feedback.Deslikes);
    Assert.AreEqual(_defaultFeedback.CustomerId, feedback.CustomerId);
    Assert.AreEqual(_defaultFeedback.ReservationId, feedback.ReservationId);
    Assert.AreEqual(_defaultFeedback.RoomId, feedback.RoomId);

  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null, null, _defaultFeedback.Comment, null, null, null, null,null,null,null, null, null, null, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);

    var feedback = feedbacks.ToList()[0];

    Assert.IsNotNull(feedback);
    Assert.AreEqual(_defaultFeedback.Id, feedback.Id);
    Assert.AreEqual(_defaultFeedback.Comment, feedback.Comment);
    Assert.AreEqual(_defaultFeedback.Rate, feedback.Rate);
    Assert.AreEqual(_defaultFeedback.Likes, feedback.Likes);
    Assert.AreEqual(_defaultFeedback.Deslikes, feedback.Deslikes);
    Assert.AreEqual(_defaultFeedback.CustomerId, feedback.CustomerId);
    Assert.AreEqual(_defaultFeedback.ReservationId, feedback.ReservationId);
    Assert.AreEqual(_defaultFeedback.RoomId, feedback.RoomId);
  }

  [TestMethod]
  public async Task GetAsync_WhereCommentIncludesServico_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null, null, "serviço", null, null, null, null, null, null, null, null, null, null, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.IsTrue(feedback.Comment.Contains("serviço"));
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereRateGratherThan5_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null, null, null, 5, "gt", null, null, null, null, null, null, null, null, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.IsTrue(5 < feedback.Rate);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereRateLessThan5_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null, null, null, 5, "lt", null, null, null, null, null, null, null, null, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.IsTrue(5 > feedback.Rate);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereRateEquals6_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null, null, null, 6, "eq", null, null, null, null, null, null, null, null, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.AreEqual(6,feedback.Rate);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereLikesGratherThan5_ReturnsFeedbacks()
  {

    var parameters = new FeedbackQueryParameters(0, 100, null, null, null,null,null, 5, "gt", null, null, null, null, null, null, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.IsTrue(5 < feedback.Likes);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereLikesLessThan5_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, 5, "lt", null, null, null, null, null, null, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.IsTrue(5 > feedback.Likes);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereLikesEquals6_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, 6, "eq", null, null, null, null, null, null, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.AreEqual(6, feedback.Likes);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereDeslikesGratherThan5_ReturnsFeedbacks()
  {

    var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, 5, "gt", null, null, null, null, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.IsTrue(5 < feedback.Deslikes);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereDeslikesLessThan5_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null,  null, null, 5, "lt", null, null, null, null, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.IsTrue(5 > feedback.Deslikes);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereDeslikesEquals6_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null,  null, null, 6, "eq", null, null, null, null, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.AreEqual(6, feedback.Deslikes);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereUpdatedAtGratherThanYesterdey_ReturnsFeedbacks()
  {

    var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, null,null ,DateTime.Now.AddDays(-1) , "gt", null, null, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.IsTrue(DateTime.Now.AddDays(-1) < feedback.UpdatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereUpdatedAtLessThanToday_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, DateTime.Now, "lt", null, null, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.IsTrue(DateTime.Now > feedback.UpdatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereUpdatedAtEquals_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, _defaultFeedback.UpdatedAt, "eq", null, null, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.AreEqual(_defaultFeedback.UpdatedAt, feedback.UpdatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtLessThanToday_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 1, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.IsTrue(DateTime.Now.AddDays(1) > feedback.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreayedAtLessThanToday_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 100, DateTime.Now, "lt", null, null, null, null, null, null, null, null,null, null, null, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.IsTrue(DateTime.Now > feedback.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCustomerId_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null,null, null, null, null, null, null, null, null, null, null, _customer.Id, null, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);


    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.AreEqual(_customer.Id, feedback.CustomerId);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereReservationId_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, null, null, _reservation.Id, null);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);


    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.AreEqual(_reservation.Id, feedback.ReservationId);
    }
  }


  [TestMethod]
  public async Task GetAsync_WhereRoomId_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, null, null, null, _room.Id);
    var feedbacks = await _feedbackRepository.GetAsync(parameters);


    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.AreEqual(_room.Id, feedback.RoomId);
    }
  }



}
