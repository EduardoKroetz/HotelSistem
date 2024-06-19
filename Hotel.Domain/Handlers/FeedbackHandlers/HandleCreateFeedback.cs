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

    public FeedbackHandler(IFeedbackRepository feedbackRepository, ICustomerRepository customerRepository, IReservationRepository reservationRepository, IRoomRepository roomRepository, ILikeRepository likeRepository, IDeslikeRepository dislikeRepository)
    {
        _feedbackRepository = feedbackRepository;
        _customerRepository = customerRepository;
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
        _likeRepository = likeRepository;
        _dislikeRepository = dislikeRepository;
    }

    public async Task<Response> HandleCreateAsync(CreateFeedback model, Guid userId)
    {
        var customer = await _customerRepository.GetEntityByIdAsync(userId);
        if (customer == null)
            throw new ArgumentException("Usu�rio n�o encontrado.");

        var reservation = await _reservationRepository.GetReservationIncludesCustomer(model.ReservationId);
        if (reservation == null)
            throw new ArgumentException("Reserva n�o encontrada.");

        var room = await _roomRepository.GetEntityByIdAsync(reservation.RoomId);
        if (room == null)
            throw new ArgumentException("Hospedagem n�o encontrada.");

        var feedback = new Feedback(
          model.Comment, model.Rate, customer.Id, model.ReservationId, room.Id, reservation //vai validar se o cliente que est� criando o feedback est� na reserva
        );

        await _feedbackRepository.CreateAsync(feedback);
        await _feedbackRepository.SaveChangesAsync();

        return new Response("Feedback criado com sucesso!", new { feedback.Id });
    }
}