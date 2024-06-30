using Hotel.Domain.Data;
using Hotel.Domain.DTOs.FeedbackDTOs;
using Hotel.Domain.Repositories;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase.Utils;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase;
using Microsoft.EntityFrameworkCore.Storage;
using Hotel.Domain.Entities.FeedbackEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.DislikeEntity;
using Hotel.Domain.Entities.LikeEntity;

namespace Hotel.Tests.UnitTests.Repositories;

[TestClass]
public class FeedbackRepositoryTest
{
    private readonly FeedbackRepository _feedbackRepository;
    private readonly HotelDbContext _dbContext;
    private readonly RepositoryTestUtils _utils;
    private static readonly AsyncLocal<IDbContextTransaction?> _currentTransaction = new AsyncLocal<IDbContextTransaction?>();

    public FeedbackRepositoryTest()
    {
        var sqliteDatabase = new SqliteDatabase();
        _dbContext = sqliteDatabase.Context;
        _feedbackRepository = new FeedbackRepository(_dbContext);
        _utils = new RepositoryTestUtils(_dbContext);
    }

    [TestInitialize]
    public async Task Initialize()
    {
        _currentTransaction.Value = await _dbContext.Database.BeginTransactionAsync();
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        if (_currentTransaction.Value != null)
        {
            await _currentTransaction.Value.RollbackAsync();
            await _currentTransaction.Value.DisposeAsync();
            _currentTransaction.Value = null;
        }
    }

    private async Task<Feedback> CreateFeedbackAsync(Customer? customer = null, Room? room = null, Reservation? reservation = null)
    {
        var newCustomer = await _utils.CreateCustomerAsync(customer ?? new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(room ?? new Room("Deluxe Beach", 99, 170, 8, "Deluxe beach is a room", newCategory));
        var newReservation = await _utils.CreateReservationAsync(reservation ?? new Reservation(newRoom, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), newCustomer, 4));
        return await _utils.CreateFeedbackAsync(new Feedback("Muito interessante o cômodo! Fiquei alguns dias hospedado e foi uma experiência incrível. O Servico é espatacular!", 8, newCustomer.Id, newReservation.Id, newRoom.Id));
    }


    //Tests

    [TestMethod]
    public async Task GetByIdAsync_ReturnsWithCorrectParameters()
    {
        //Arrange
        var newFeedback = await CreateFeedbackAsync();

        //Act
        var feedback = await _feedbackRepository.GetByIdAsync(newFeedback.Id);

        //Assert
        Assert.IsNotNull(feedback);
        Assert.AreEqual(newFeedback.Id, feedback.Id);
        Assert.AreEqual(newFeedback.Comment, feedback.Comment);
        Assert.AreEqual(newFeedback.Rate, feedback.Rate);
        Assert.AreEqual(newFeedback.Likes.Count, feedback.Likes);
        Assert.AreEqual(newFeedback.Dislikes.Count, feedback.Dislikes);
        Assert.AreEqual(newFeedback.CustomerId, feedback.CustomerId);
        Assert.AreEqual(newFeedback.ReservationId, feedback.ReservationId);
        Assert.AreEqual(newFeedback.RoomId, feedback.RoomId);
    }

    [TestMethod]
    public async Task GetAsync_ReturnWithCorrectParameters()
    {
        //Arrange
        var newFeedback = await CreateFeedbackAsync();
        var parameters = new FeedbackQueryParameters{ Comment = newFeedback.Comment };
        
        //Act
        var feedbacks = await _feedbackRepository.GetAsync(parameters);

        //Assert
        var feedback = feedbacks.ToList()[0];

        Assert.IsNotNull(feedback);
        Assert.AreEqual(newFeedback.Id, feedback.Id);
        Assert.AreEqual(newFeedback.Comment, feedback.Comment);
        Assert.AreEqual(newFeedback.Rate, feedback.Rate);
        Assert.AreEqual(newFeedback.Likes.Count, feedback.Likes);
        Assert.AreEqual(newFeedback.Dislikes.Count, feedback.Dislikes);
        Assert.AreEqual(newFeedback.CustomerId, feedback.CustomerId);
        Assert.AreEqual(newFeedback.ReservationId, feedback.ReservationId);
        Assert.AreEqual(newFeedback.RoomId, feedback.RoomId);
    }

    [TestMethod]
    public async Task GetAsync_WhereComment_ReturnsFeedbacks()
    {
        //Arrange
        var newFeedback = await CreateFeedbackAsync();
        var parameters = new FeedbackQueryParameters{ Comment = "interessante" };
        
        //Act
        var feedbacks = await _feedbackRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(feedback.Comment.Contains("interessante"));

    }

    [TestMethod]
    public async Task GetAsync_WhereRateGratherThan5_ReturnsFeedbacks()
    {
        //Arrange
        var newFeedback = await CreateFeedbackAsync();
        var parameters = new FeedbackQueryParameters{ Rate = 5, RateOperator = "gt" };
        
        //Act
        var feedbacks = await _feedbackRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(5 < feedback.Rate);

    }

    [TestMethod]
    public async Task GetAsync_WhereRateLessThan5_ReturnsFeedbacks()
    {
        //Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Deluxe Beach", 99, 170, 8, "Deluxe beach is a room", newCategory));
        var newReservation = await _utils.CreateReservationAsync(new Reservation(newRoom, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), newCustomer, 4));
        var newFeedback = await _utils.CreateFeedbackAsync(new Feedback("Ruim", 4, newCustomer.Id, newReservation.Id, newRoom.Id));
        var parameters = new FeedbackQueryParameters{ Rate = 5, RateOperator = "lt" };
        
        //Act
        var feedbacks = await _feedbackRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(5 > feedback.Rate);

    }

    [TestMethod]
    public async Task GetAsync_WhereRateEquals6_ReturnsFeedbacks()
    {
        //Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Deluxe Beach", 99, 170, 8, "Deluxe beach is a room", newCategory));
        var newReservation = await _utils.CreateReservationAsync(new Reservation(newRoom, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), newCustomer, 4));
        var newFeedback = await _utils.CreateFeedbackAsync(new Feedback("Legal", 6, newCustomer.Id, newReservation.Id, newRoom.Id));
        var parameters = new FeedbackQueryParameters{ Rate = 6, RateOperator = "eq" };
        
        //Act
        var feedbacks = await _feedbackRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.AreEqual(6, feedback.Rate);

    }

    [TestMethod]
    public async Task GetAsync_WhereLikesGratherThan1_ReturnsFeedbacks()
    {
        //Arrange
        var newFeedback = await CreateFeedbackAsync();
        await _utils.CreateLikeAsync(new Like(new Customer(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98365-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Pauki", "Av. Sp", 999)), newFeedback));
        await _utils.CreateLikeAsync(new Like(new Customer(new Name("Maria", "Silva"), new Email("maria.silva@example.com"), new Phone("+55 (11) 98125-4321"), "senha123", EGender.Feminine, DateTime.Now.AddYears(-25), new Address("Brazil", "Rio de Janeiro", "Rua Copacabana", 123)), newFeedback));
        var parameters = new FeedbackQueryParameters{ Likes = 1, LikesOperator = "gt" };
        
        //Act
        var feedbacks = await _feedbackRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(1 < feedback.Likes);

    }

    [TestMethod]
    public async Task GetAsync_WhereLikesLessThan2_ReturnsFeedbacks()
    {
        //Arrange
        var newFeedback = await CreateFeedbackAsync();
        var parameters = new FeedbackQueryParameters{ Likes = 2, LikesOperator = "lt" };
        
        //Act
        var feedbacks = await _feedbackRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(2 > feedback.Likes);

    }

    [TestMethod]
    public async Task GetAsync_WhereLikesEquals1_ReturnsFeedbacks()
    {
        //Arrange
        var newFeedback = await CreateFeedbackAsync();
        await _utils.CreateLikeAsync(new Like(new Customer(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98365-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Pauki", "Av. Sp", 999)), newFeedback));
        var parameters = new FeedbackQueryParameters{ Likes = 1, LikesOperator = "eq" };
        
        //Act
        var feedbacks = await _feedbackRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.AreEqual(1, feedback.Likes);

    }

    [TestMethod]
    public async Task GetAsync_WhereDislikesGratherThan1_ReturnsFeedbacks()
    {
        //Arrange
        var newFeedback = await CreateFeedbackAsync();
        await _utils.CreateDislikeAsync(new Dislike(new Customer(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98365-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Pauki", "Av. Sp", 999)), newFeedback));
        await _utils.CreateDislikeAsync(new Dislike(new Customer(new Name("Maria", "Silva"), new Email("maria.silva@example.com"), new Phone("+55 (11) 98125-4321"), "senha123", EGender.Feminine, DateTime.Now.AddYears(-25), new Address("Brazil", "Rio de Janeiro", "Rua Copacabana", 123)), newFeedback));
        var parameters = new FeedbackQueryParameters{ Dislikes = 1, DislikesOperator = "gt" };
        
        //Act
        var feedbacks = await _feedbackRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(1 < feedback.Dislikes);

    }

    [TestMethod]
    public async Task GetAsync_WhereDislikesLessThan2_ReturnsFeedbacks()
    {
        //Arrange
        var newFeedback = await CreateFeedbackAsync();
        var parameters = new FeedbackQueryParameters{ Dislikes = 2, DislikesOperator = "lt" };
        
        //Act
        var feedbacks = await _feedbackRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(2 > feedback.Dislikes);

    }

    [TestMethod]
    public async Task GetAsync_WhereDislikesEquals1_ReturnsFeedbacks()
    {
        //Arrange
        var newCustomer = new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3));
        var newFeedback = await CreateFeedbackAsync(newCustomer);
        await _utils.CreateDislikeAsync(new Dislike(newCustomer, newFeedback));

        var parameters = new FeedbackQueryParameters{ Dislikes = 1, DislikesOperator = "eq" };
        //Act
        var feedbacks = await _feedbackRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.AreEqual(1, feedback.Dislikes);

    }

    [TestMethod]
    public async Task GetAsync_WhereUpdatedAtGratherThanYesterdey_ReturnsFeedbacks()
    {

        //Arrange
        var newFeedback = await CreateFeedbackAsync();
        var parameters = new FeedbackQueryParameters{ UpdatedAt = DateTime.Now.AddDays(-1), UpdatedAtOperator = "gt" };
        
        //Act
        var feedbacks = await _feedbackRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(DateTime.Now.AddDays(-1) < feedback.UpdatedAt);

    }

    [TestMethod]
    public async Task GetAsync_WhereUpdatedAtLessThanToday_ReturnsFeedbacks()
    {
        //Arrange
        var newFeedback = await CreateFeedbackAsync();
        var parameters = new FeedbackQueryParameters{ UpdatedAt = DateTime.Now, UpdatedAtOperator = "lt" };
        
        //Act
        var feedbacks = await _feedbackRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.IsTrue(DateTime.Now > feedback.UpdatedAt);

    }

    [TestMethod]
    public async Task GetAsync_WhereUpdatedAtEquals_ReturnsFeedbacks()
    {
        //Arrange
        var newFeedback = await CreateFeedbackAsync();
        var parameters = new FeedbackQueryParameters{ UpdatedAt = newFeedback.UpdatedAt, UpdatedAtOperator = "eq" };
        
        //Act
        var feedbacks = await _feedbackRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.AreEqual(newFeedback.UpdatedAt, feedback.UpdatedAt);

    }


    [TestMethod]
    public async Task GetAsync_WhereCustomerId_ReturnsFeedbacks()
    {
        //Arrange
        var newCustomer = new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3));
        var newFeedback = await CreateFeedbackAsync(newCustomer);
        var parameters = new FeedbackQueryParameters{ CustomerId =  newCustomer.Id };
        
        //Act
        var feedbacks = await _feedbackRepository.GetAsync(parameters);

        //Assert

        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.AreEqual(newCustomer.Id, feedback.CustomerId);

    }

    [TestMethod]
    public async Task GetAsync_WhereReservationId_ReturnsFeedbacks()
    {
        //Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Deluxe Beach", 99, 170, 8, "Deluxe beach is a room", newCategory));
        var newReservation = await _utils.CreateReservationAsync(new Reservation(newRoom, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), newCustomer, 4));
        var newFeedback = await _utils.CreateFeedbackAsync(new Feedback("Ruim", 4, newCustomer.Id, newReservation.Id, newRoom.Id));
        var parameters = new FeedbackQueryParameters{ ReservationId = newReservation.Id };
        
        //Act
        var feedbacks = await _feedbackRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.AreEqual(newReservation.Id, feedback.ReservationId);

    }


    [TestMethod]
    public async Task GetAsync_WhereRoomId_ReturnsFeedbacks()
    {
        //Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Deluxe Beach", 99, 170, 8, "Deluxe beach is a room", newCategory));
        var newReservation = await _utils.CreateReservationAsync(new Reservation(newRoom, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), newCustomer, 4));
        var newFeedback = await _utils.CreateFeedbackAsync(new Feedback("Ruim", 4, newCustomer.Id, newReservation.Id, newRoom.Id));
        var parameters = new FeedbackQueryParameters { RoomId = newRoom.Id };
        
        //Act
        var feedbacks = await _feedbackRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(feedbacks.Any());
        foreach (var feedback in feedbacks)
            Assert.AreEqual(newRoom.Id, feedback.RoomId);

    }

}
