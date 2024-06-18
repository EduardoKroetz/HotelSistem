using Hotel.Domain.Data;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.FeedbackEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Services.TokenServices;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.IntegrationTests.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Hotel.Domain.DTOs.FeedbackDTOs;
using Hotel.Domain.Entities.LikeEntity;
using Hotel.Domain.Entities.DislikeEntity;

namespace Hotel.Tests.IntegrationTests.Controllers;

[TestClass]
public class FeedbackControllerTests
{
    private static HotelWebApplicationFactory _factory = null!;
    private static HttpClient _client = null!;
    private static HotelDbContext _dbContext = null!;
    private const string _baseUrl = "v1/feedbacks";
    private static TokenService _tokenService = null!;
    private static List<Feedback> _feedbacks = [];
    private static string _customerToken = null!;

    [ClassInitialize]
    public static void ClassInitialize(TestContext? context)
    {
        _factory = new HotelWebApplicationFactory();
        _client = _factory.CreateClient();
        _dbContext = _factory.Services.GetRequiredService<HotelDbContext>();
        _tokenService = _factory.Services.GetRequiredService<TokenService>();

        SeedFeedbacks().Wait();
        _feedbacks = _dbContext.Feedbacks.ToListAsync().Result;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _customerToken);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _factory.Dispose();
    }

    [TestInitialize]
    public void TestInitialize()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _customerToken);
    }

    public static async Task SeedFeedbacks()
    {
        var customer = new Customer
        (
          new Name("Jennifer", "Lawrence"),
          new Email("jenniferLawrenceOfficial@gmail.com"),
          new Phone("+44 (20) 97890-1234"),
          "789",
          EGender.Feminine,
          DateTime.Now.AddYears(-30),
          new Address("United States", "Los Angeles", "US-456", 789)
        );
        var category = new Category("Quartos standard", "Quartos padrões", 40);
        var room = new Room(1, 36, 3, "Quarto padrão", category.Id);
        var reservation = new Reservation(room, DateTime.Now.AddDays(3), DateTime.Now.AddDays(6), customer, 2);
        var feedback = new Feedback("Gostei muito do quarto!", 9, customer.Id, reservation.Id, room.Id, reservation);


        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.Feedbacks.AddAsync(feedback);
        await _dbContext.SaveChangesAsync();

        _customerToken = _tokenService.GenerateToken(customer);
    }

    //TESTS
    [TestMethod]
    public async Task CreateFeedback_ShouldReturn_OK()
    {
        // Arange
        var customer = new Customer
        (
            new Name("Gabriela", "Santos"),
            new Email("gabrielaSantos123@gmail.com"),
            new Phone("+55 (81) 98765-4321"),
            "654",
            EGender.Feminine,
            DateTime.Now.AddYears(-32),
            new Address("Brazil", "Recife", "BR-456", 789)
        );

        var category = new Category("Suíte de Luxo", "Suítes elegantes", 180);
        var room = new Room(201, 180, 3, "Suíte de Luxo", category.Id);
        var reservation = new Reservation(room, DateTime.Now.AddDays(8), DateTime.Now.AddDays(12), customer, 2);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GenerateToken(customer));

        var body = new CreateFeedback("Estadia perfeita, serviço impecável!", 10, reservation.Id);

        //Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        //Assert
        var feedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(x => x.Comment.Equals(body.Comment));

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(body.Rate, feedback!.Rate);
        Assert.AreEqual(body.Comment, feedback.Comment);
        Assert.AreEqual(reservation.Id, feedback.ReservationId);
    }


    [TestMethod]
    public async Task GetFeedbacks_ShouldReturn_OK()
    {
        //Act
        var response = await _client.GetAsync($"{_baseUrl}?take=1");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task GetFeedbackById_ShouldReturn_OK()
    {
        //Act
        var response = await _client.GetAsync($"{_baseUrl}/{_feedbacks[0].Id}");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task DeleteFeedback_ShouldReturn_OK()
    {
        //Arange
        var customer = new Customer
        (
          new Name("Lucas", "Lawrence"),
          new Email("lucasLawrenceOfficial@gmail.com"),
          new Phone("+44 (20) 97193-1624"),
          "789",
          EGender.Feminine,
          DateTime.Now.AddYears(-30),
          new Address("United States", "Los Angeles", "US-456", 789)
        );
        var category = new Category("Quarto de luxo", "Quartos de luxo", 80);
        var room = new Room(5, 89, 3, "Quarto de luxo", category.Id);
        var reservation = new Reservation(room, DateTime.Now.AddDays(3), DateTime.Now.AddDays(6), customer, 2);
        var feedback = new Feedback("Gostei muito do quarto!", 9, customer.Id, reservation.Id, room.Id, reservation);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.Feedbacks.AddAsync(feedback);
        await _dbContext.SaveChangesAsync();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GenerateToken(customer));

        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{feedback.Id}");

        //Assert
        var wasNotDeleted = await _dbContext.Feedbacks.AnyAsync(x => x.Id == feedback.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsFalse(wasNotDeleted);
    }


    [TestMethod]
    public async Task UpdateFeedback_ShouldReturn_OK()
    {
        //Arange
        var customer = new Customer
        (
          new Name("Maria", "Silva"),
          new Email("mariaSilvaOfficial@gmail.com"),
          new Phone("+55 (11) 91214-5578"),
          "123",
          EGender.Feminine,
          DateTime.Now.AddYears(-28),
          new Address("Brazil", "São Paulo", "BR-123", 456)
        );

        var category = new Category("Suíte Relux", "Suítes de luxo", 150);
        var room = new Room(8, 150, 5, "Suíte Relux", category.Id);
        var reservation = new Reservation(room, DateTime.Now.AddDays(7), DateTime.Now.AddDays(10), customer, 4);
        var feedback = new Feedback("Legal, recomendo!", 10, customer.Id, reservation.Id, room.Id, reservation);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.Feedbacks.AddAsync(feedback);
        await _dbContext.SaveChangesAsync();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GenerateToken(customer));

        var body = new UpdateFeedback("A suite é de outro mundo, uma das melhores que já fui, recomendo!", 10);

        //Act
        var response = await _client.PutAsJsonAsync($"{_baseUrl}/{feedback.Id}", body);

        //Assert
        var updatedFeedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(x => x.Id == feedback.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(updatedFeedback!.Id, feedback.Id);
        Assert.AreEqual(updatedFeedback.Comment, body.Comment);
        Assert.AreEqual(updatedFeedback.Rate, body.Rate);
    }


    [TestMethod]
    public async Task UpdateFeedbackComment_ShouldReturn_OK()
    {
        // Arange
        var customer = new Customer
        (
          new Name("João", "Pereira"),
          new Email("joaoPereira123@gmail.com"),
          new Phone("+55 (21) 99876-5432"),
          "456",
          EGender.Masculine,
          DateTime.Now.AddYears(-35),
          new Address("Brazil", "Rio de Janeiro", "BR-789", 321)
        );
        var category = new Category("Apartamento Executivo", "Apartamentos executivos", 120);
        var room = new Room(22, 120, 10, "Apartamento Executivo", category.Id);
        var reservation = new Reservation(room, DateTime.Now.AddDays(15), DateTime.Now.AddDays(20), customer, 1);
        var feedback = new Feedback("Serviço de qualidade, apartamento confortável.", 8, customer.Id, reservation.Id, room.Id, reservation);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.Feedbacks.AddAsync(feedback);
        await _dbContext.SaveChangesAsync();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GenerateToken(customer));

        var body = new UpdateComment("Serviço é bom e o apartamento é confotável. Entrega o que promete.");

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{feedback.Id}/comment", body);

        //Assert
        var updatedFeedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(x => x.Id == feedback.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(updatedFeedback!.Comment, body.Comment);
    }

    [TestMethod]
    public async Task UpdateFeedbackRate_ShouldReturn_OK()
    {
        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{_feedbacks[0].Id}/rate/8", new { });

        //Assert
        var updatedFeedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(x => x.Id == _feedbacks[0].Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(updatedFeedback!.Rate, 8);
    }

    [TestMethod]
    public async Task AddLikeToFeedback_ShouldReturn_OK()
    {
        // Arange
        var customer = new Customer
        (
          new Name("Ana", "Souza"),
          new Email("anaSouza456@gmail.com"),
          new Phone("+55 (31) 98765-4123"),
          "789",
          EGender.Feminine,
          DateTime.Now.AddYears(-29),
          new Address("Brazil", "Belo Horizonte", "BR-321", 654)
        );
        var category = new Category("Suíte Master", "Suítes de alta classe", 200);
        var room = new Room(45, 200, 12, "Suíte Master", category.Id);
        var reservation = new Reservation(room, DateTime.Now.AddDays(10), DateTime.Now.AddDays(14), customer, 5);
        var feedback = new Feedback("Excelente suíte, recomendo fortemente.", 10, customer.Id, reservation.Id, room.Id, reservation);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.Feedbacks.AddAsync(feedback);
        await _dbContext.SaveChangesAsync();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GenerateToken(customer));

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/add-like/{feedback.Id}", new { });

        //Assert
        var updatedFeedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(x => x.Id == feedback.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(updatedFeedback!.Likes.Count, 1);
    }

    [TestMethod]
    public async Task RemoveLikeFromFeedback_ShouldReturn_OK()
    {
        // Arange
        var customer = new Customer
        (
          new Name("Carlos", "Mendes"),
          new Email("carlosMendes789@gmail.com"),
          new Phone("+55 (41) 99576-5432"),
          "321",
          EGender.Masculine,
          DateTime.Now.AddYears(-40),
          new Address("Brazil", "Curitiba", "BR-654", 987)
        );
        var category = new Category("Cabana de Luxo", "Cabanas exclusivas", 250);
        var room = new Room(101, 250, 4, "Cabana de Luxo", category.Id);
        var reservation = new Reservation(room, DateTime.Now.AddDays(5), DateTime.Now.AddDays(9), customer, 3);
        var feedback = new Feedback("Lugar maravilhoso e tranquilo.", 9, customer.Id, reservation.Id, room.Id, reservation);

        var like = new Like(customer, feedback);
        feedback.Likes.Add(like);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.Feedbacks.AddAsync(feedback);
        await _dbContext.SaveChangesAsync();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GenerateToken(customer));

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/remove-like/{feedback.Id}", new { });

        //Assert
        var updatedFeedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(x => x.Id == feedback.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(updatedFeedback!.Likes.Count, 0);
    }

    [TestMethod]
    public async Task RemoveLikeFromFeedback_WhenNotYourOwn_ShouldReturn_NotFound()
    {
        // Arange
        var customer = new Customer
      (
          new Name("Mariana", "Oliveira"),
          new Email("marianaOliveira654@gmail.com"),
          new Phone("+55 (71) 91233-5978"),
          "987",
          EGender.Feminine,
          DateTime.Now.AddYears(-27),
          new Address("Brazil", "Salvador", "BR-987", 432)
        );

        var category = new Category("Villa Privativa", "Villas exclusivas", 300);
        var room = new Room(303, 300, 6, "Villa Privativa", category.Id);
        var reservation = new Reservation(room, DateTime.Now.AddDays(12), DateTime.Now.AddDays(17), customer, 4);
        var feedback = new Feedback("Experiência fantástica, altamente recomendado!", 10, customer.Id, reservation.Id, room.Id, reservation);
        var like = new Like(customer, feedback);
        feedback.Likes.Add(like);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.Feedbacks.AddAsync(feedback);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/remove-like/{feedback.Id}", new { });

        //Assert
        var updatedFeedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(x => x.Id == feedback.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        Assert.AreEqual(1, updatedFeedback!.Likes.Count);
    }

    [TestMethod]
    public async Task AddDislikeToFeedback_ShouldReturn_OK()
    {
        // Arranjo
        var customer = new Customer
        (
            new Name("Maria", "Silva"),
            new Email("mariaSilva789@gmail.com"),
            new Phone("+55 (31) 98715-4321"),
            "123",
            EGender.Feminine,
            DateTime.Now.AddYears(-35),
            new Address("Brazil", "São Paulo", "BR-123", 456)
        );
        var category = new Category("Quarto Executivo", "Quartos de luxo", 300);
        var room = new Room(20, 300, 10, "Quarto Executivo", category.Id);
        var reservation = new Reservation(room, DateTime.Now.AddDays(7), DateTime.Now.AddDays(12), customer, 2);
        var feedback = new Feedback("Ótimo serviço, mas o café da manhã pode melhorar.", 4, customer.Id, reservation.Id, room.Id, reservation);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.Feedbacks.AddAsync(feedback);
        await _dbContext.SaveChangesAsync();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GenerateToken(customer));

        // Atua
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/add-dislike/{feedback.Id}", new { });

        // Afirmação
        var updatedFeedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(x => x.Id == feedback.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(1, updatedFeedback!.Dislikes.Count);
    }

    [TestMethod]
    public async Task RemoveDislikeFromFeedback_ShouldReturn_OK()
    {
        // Arrange
        var customer = new Customer
        (
          new Name("Lucas", "Almeida"),
          new Email("lucasAlmeida321@gmail.com"),
          new Phone("+55 (41) 99876-5132"),
          "654",
          EGender.Masculine,
          DateTime.Now.AddYears(-28),
          new Address("Brazil", "Porto Alegre", "BR-654", 321)
        );
        var category = new Category("Suíte Presidencial", "Suítes de alto padrão", 500);
        var room = new Room(30, 500, 8, "Suíte Presidencial", category.Id);
        var reservation = new Reservation(room, DateTime.Now.AddDays(14), DateTime.Now.AddDays(20), customer, 3);
        var feedback = new Feedback("Atendimento impecável e vista deslumbrante.", 10, customer.Id, reservation.Id, room.Id, reservation);

        var dislike = new Dislike(customer, feedback);
        feedback.Dislikes.Add(dislike);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.Feedbacks.AddAsync(feedback);
        await _dbContext.SaveChangesAsync();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GenerateToken(customer));

        // Atua
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/remove-dislike/{feedback.Id}", new { });

        // Afirmação
        var updatedFeedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(x => x.Id == feedback.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(0, updatedFeedback!.Dislikes.Count);
    }


    [TestMethod]
    public async Task RemoveDislikeFromFeedback_WhenNotYourOwn_ShouldReturn_NOTFOUND()
    {
        // Arrange
        var customer = new Customer
        (
          new Name("Luiz", "Silva"),
          new Email("luizSilva123@gmail.com"),
          new Phone("+55 (11) 91225-5678"),
          "123",
          EGender.Masculine,
          DateTime.Now.AddYears(-30),
          new Address("Brazil", "São Paulo", "BR-123", 456)
        );
        var category = new Category("Views suites", "Suítes com vistas elegantes", 180);
        var room = new Room(19, 180, 3, "Suite média", category.Id);
        var reservation = new Reservation(room, DateTime.Now.AddDays(8), DateTime.Now.AddDays(12), customer, 2);
        var feedback = new Feedback("Estadia perfeita, serviço impecável!", 10, customer.Id, reservation.Id, room.Id, reservation);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.Feedbacks.AddAsync(feedback);
        await _dbContext.SaveChangesAsync();

        var dislike = new Dislike(customer, feedback);
        feedback.Dislikes.Add(dislike);

        // Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/remove-dislike/{feedback.Id}", new { });

        // Assert
        var updatedFeedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(x => x.Id == feedback.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        Assert.AreEqual(1, updatedFeedback!.Dislikes.Count);
    }
}
