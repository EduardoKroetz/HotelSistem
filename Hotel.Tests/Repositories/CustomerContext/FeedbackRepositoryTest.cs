using Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;
using Hotel.Domain.Repositories.CustomerContext;
using Hotel.Tests.Repositories.Mock;


namespace Hotel.Tests.Repositories.CustomerContext;

[TestClass]
public class FeedbackRepositoryTest
{
  private static FeedbackRepository FeedbackRepository { get; set; }

  static FeedbackRepositoryTest()
  => FeedbackRepository = new FeedbackRepository(BaseRepositoryTest.MockConnection.Context);

  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var feedback = await FeedbackRepository.GetByIdAsync(BaseRepositoryTest.Feedbacks[0].Id);

    Assert.IsNotNull(feedback);
    Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].Id, feedback.Id);
    Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].Comment, feedback.Comment);
    Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].Rate, feedback.Rate);
    Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].Likes, feedback.Likes);
    Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].Deslikes, feedback.Deslikes);
    Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].CustomerId, feedback.CustomerId);
    Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].ReservationId, feedback.ReservationId);
    Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].RoomId, feedback.RoomId);

  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null, null, BaseRepositoryTest.Feedbacks[0].Comment, null, null, null, null,null,null,null, null, null, null, null);
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

    var feedback = feedbacks.ToList()[0];

    Assert.IsNotNull(feedback);
    Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].Id, feedback.Id);
    Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].Comment, feedback.Comment);
    Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].Rate, feedback.Rate);
    Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].Likes, feedback.Likes);
    Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].Deslikes, feedback.Deslikes);
    Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].CustomerId, feedback.CustomerId);
    Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].ReservationId, feedback.ReservationId);
    Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].RoomId, feedback.RoomId);
  }

  [TestMethod]
  public async Task GetAsync_WhereCommentIncludesServico_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null, null, "serviço", null, null, null, null, null, null, null, null, null, null, null);
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

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
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

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
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

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
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

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
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

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
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

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
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

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
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

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
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

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
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

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
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

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
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

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
    var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, BaseRepositoryTest.Feedbacks[0].UpdatedAt, "eq", null, null, null);
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].UpdatedAt, feedback.UpdatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtLessThanToday_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 1, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

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
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

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
    var parameters = new FeedbackQueryParameters(0, 100, null,null, null, null, null, null, null, null, null, null, null, BaseRepositoryTest.Customers[0].Id, null, null);
    var feedbacks = await FeedbackRepository.GetAsync(parameters);


    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.AreEqual(BaseRepositoryTest.Customers[0].Id, feedback.CustomerId);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereReservationId_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, null, null, BaseRepositoryTest.Reservations[0].Id, null);
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.AreEqual(BaseRepositoryTest.Reservations[0].Id, feedback.ReservationId);
    }
  }


  [TestMethod]
  public async Task GetAsync_WhereRoomId_ReturnsFeedbacks()
  {
    var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, null, null, null, BaseRepositoryTest.Rooms[0].Id);
    var feedbacks = await FeedbackRepository.GetAsync(parameters);

    Assert.IsTrue(feedbacks.Any());
    foreach (var feedback in feedbacks)
    {
      Assert.IsNotNull(feedback);
      Assert.AreEqual(BaseRepositoryTest.Rooms[0].Id, feedback.RoomId);
    }
  }



}
