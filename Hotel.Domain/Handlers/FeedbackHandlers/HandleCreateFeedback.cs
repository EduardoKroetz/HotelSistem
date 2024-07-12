using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.FeedbackDTOs;
using Hotel.Domain.Entities.FeedbackEntity;
using Hotel.Domain.Handlers.Base.Interfaces;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Handlers.FeedbackHandlers;

public partial class FeedbackHandler : IHandler
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly IDeslikeRepository _dislikeRepository;
    private readonly ILogger<FeedbackHandler> _logger;


    public FeedbackHandler(IFeedbackRepository feedbackRepository, ICustomerRepository customerRepository, IReservationRepository reservationRepository, IRoomRepository roomRepository, ILikeRepository likeRepository, IDeslikeRepository dislikeRepository, ILogger<FeedbackHandler> logger)
    {
        _feedbackRepository = feedbackRepository;
        _customerRepository = customerRepository;
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
        _likeRepository = likeRepository;
        _dislikeRepository = dislikeRepository;
        _logger = logger;
    }

    public async Task<Response> HandleCreateAsync(CreateFeedback model, Guid userId)
    {
        var customer = await _customerRepository.GetEntityByIdAsync(userId);
        if (customer == null)
            throw new ArgumentException("Usuário não encontrado.");

        var reservation = await _reservationRepository.GetReservationIncludesCustomer(model.ReservationId);
        if (reservation == null)
            throw new ArgumentException("Reserva não encontrada.");

        var room = await _roomRepository.GetEntityByIdAsync(reservation.RoomId);
        if (room == null)
            throw new ArgumentException("Hospedagem não encontrada.");

        var feedback = new Feedback(
          model.Comment, model.Rate, customer.Id, model.ReservationId, room.Id, reservation //vai validar se o cliente que está criando o feedback está na reserva
        );

        try
        {
            await _feedbackRepository.CreateAsync(feedback);
            await _feedbackRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ocorreu um erro ao criar o feedback no banco de dados: {ex.Message}");
        }


        return new Response("Feedback criado com sucesso!", new { feedback.Id });
    }
}