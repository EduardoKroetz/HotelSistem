﻿using Hotel.Domain.DTOs.FeedbackDTOs;
using Hotel.Domain.Repositories;
using Hotel.Tests.UnitTests.Repositories.Mock;


namespace Hotel.Tests.UnitTests.Repositories;

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
        Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].Likes.Count, feedback.Likes);
        Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].Dislikes.Count, feedback.Dislikes);
        Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].CustomerId, feedback.CustomerId);
        Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].ReservationId, feedback.ReservationId);
        Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].RoomId, feedback.RoomId);
    }

    [TestMethod]
    public async Task GetAsync_ReturnWithCorrectParameters()
    {
        var parameters = new FeedbackQueryParameters(0, 100, null, null, BaseRepositoryTest.Feedbacks[0].Comment, null, null, null, null, null, null, null, null, null, null, null);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);

        var feedback = feedbacks.ToList()[0];

        Assert.IsNotNull(feedback);
        Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].Id, feedback.Id);
        Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].Comment, feedback.Comment);
        Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].Rate, feedback.Rate);
        Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].Likes.Count, feedback.Likes);
        Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].Dislikes.Count, feedback.Dislikes);
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
            Assert.IsTrue(feedback.Comment.Contains("serviço"));

    }

    [TestMethod]
    public async Task GetAsync_WhereRateGratherThan5_ReturnsFeedbacks()
    {
        var parameters = new FeedbackQueryParameters(0, 100, null, null, null, 5, "gt", null, null, null, null, null, null, null, null, null);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);

        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(5 < feedback.Rate);

    }

    [TestMethod]
    public async Task GetAsync_WhereRateLessThan5_ReturnsFeedbacks()
    {
        var parameters = new FeedbackQueryParameters(0, 100, null, null, null, 5, "lt", null, null, null, null, null, null, null, null, null);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);

        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(5 > feedback.Rate);

    }

    [TestMethod]
    public async Task GetAsync_WhereRateEquals6_ReturnsFeedbacks()
    {
        var parameters = new FeedbackQueryParameters(0, 100, null, null, null, 6, "eq", null, null, null, null, null, null, null, null, null);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);

        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.AreEqual(6, feedback.Rate);

    }

    [TestMethod]
    public async Task GetAsync_WhereLikesGratherThan2_ReturnsFeedbacks()
    {
        var a = BaseRepositoryTest.Feedbacks;
        var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, 2, "gt", null, null, null, null, null, null, null);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);

        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(2 < feedback.Likes);

    }

    [TestMethod]
    public async Task GetAsync_WhereLikesLessThan2_ReturnsFeedbacks()
    {
        var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, 2, "lt", null, null, null, null, null, null, null);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);

        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(2 > feedback.Likes);

    }

    [TestMethod]
    public async Task GetAsync_WhereLikesEquals1_ReturnsFeedbacks()
    {
        var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, 1, "eq", null, null, null, null, null, null, null);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);

        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.AreEqual(1, feedback.Likes);

    }

    [TestMethod]
    public async Task GetAsync_WhereDislikesGratherThan2_ReturnsFeedbacks()
    {

        var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, 2, "gt", null, null, null, null, null);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);

        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(2 < feedback.Dislikes);

    }

    [TestMethod]
    public async Task GetAsync_WhereDislikesLessThan2_ReturnsFeedbacks()
    {
        var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, 2, "lt", null, null, null, null, null);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);

        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(2 > feedback.Dislikes);

    }

    [TestMethod]
    public async Task GetAsync_WhereDislikesEquals1_ReturnsFeedbacks()
    {
        var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, 1, "eq", null, null, null, null, null);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);

        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.AreEqual(1, feedback.Dislikes);

    }

    [TestMethod]
    public async Task GetAsync_WhereUpdatedAtGratherThanYesterdey_ReturnsFeedbacks()
    {

        var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, DateTime.Now.AddDays(-1), "gt", null, null, null);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);

        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(DateTime.Now.AddDays(-1) < feedback.UpdatedAt);

    }

    [TestMethod]
    public async Task GetAsync_WhereUpdatedAtLessThanToday_ReturnsFeedbacks()
    {
        var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, DateTime.Now, "lt", null, null, null);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);

        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(DateTime.Now > feedback.UpdatedAt);

    }

    [TestMethod]
    public async Task GetAsync_WhereUpdatedAtEquals_ReturnsFeedbacks()
    {
        var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, BaseRepositoryTest.Feedbacks[0].UpdatedAt, "eq", null, null, null);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);

        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.AreEqual(BaseRepositoryTest.Feedbacks[0].UpdatedAt, feedback.UpdatedAt);

    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtLessThanToday_ReturnsFeedbacks()
    {
        var parameters = new FeedbackQueryParameters(0, 1, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);

        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(DateTime.Now.AddDays(1) > feedback.CreatedAt);

    }

    [TestMethod]
    public async Task GetAsync_WhereCreayedAtLessThanToday_ReturnsFeedbacks()
    {
        var parameters = new FeedbackQueryParameters(0, 100, DateTime.Now, "lt", null, null, null, null, null, null, null, null, null, null, null, null);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);

        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(DateTime.Now > feedback.CreatedAt);

    }

    [TestMethod]
    public async Task GetAsync_WhereCustomerId_ReturnsFeedbacks()
    {
        var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, null, BaseRepositoryTest.Customers[0].Id, null, null);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);


        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.AreEqual(BaseRepositoryTest.Customers[0].Id, feedback.CustomerId);

    }

    [TestMethod]
    public async Task GetAsync_WhereReservationId_ReturnsFeedbacks()
    {
        var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, null, null, BaseRepositoryTest.Reservations[0].Id, null);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);

        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.AreEqual(BaseRepositoryTest.Reservations[0].Id, feedback.ReservationId);

    }


    [TestMethod]
    public async Task GetAsync_WhereRoomId_ReturnsFeedbacks()
    {
        var parameters = new FeedbackQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, null, null, null, BaseRepositoryTest.Rooms[0].Id);
        var feedbacks = await FeedbackRepository.GetAsync(parameters);

        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.AreEqual(BaseRepositoryTest.Rooms[0].Id, feedback.RoomId);

    }

}