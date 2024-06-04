using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces.CustomerContext;
using Hotel.Domain.Repositories.Interfaces.ReservationContext;
using Hotel.Domain.Repositories.Interfaces.RoomContext;

namespace Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;

public partial class FeedbackHandler : IHandler
{
  private readonly IFeedbackRepository  _feedbackRepository;
  private readonly ICustomerRepository _customerRepository;
  private readonly IReservationRepository _reservationRepository;
  private readonly IRoomRepository _roomRepository;
  private readonly ILikeRepository _likeRepository;
  private readonly IDeslikeRepository _deslikeRepository;

  public FeedbackHandler(IFeedbackRepository feedbackRepository, ICustomerRepository customerRepository, IReservationRepository reservationRepository, IRoomRepository roomRepository, ILikeRepository likeRepository, IDeslikeRepository deslikeRepository)
  {
    _feedbackRepository = feedbackRepository;
    _customerRepository = customerRepository;
    _reservationRepository = reservationRepository;
    _roomRepository = roomRepository;
    _likeRepository = likeRepository;
    _deslikeRepository = deslikeRepository;
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
      model.Comment,model.Rate,customer.Id,model.ReservationId,room.Id, reservation //vai validar se o cliente que está criando o feedback está na reserva
    );

    await _feedbackRepository.CreateAsync(feedback);
    await _feedbackRepository.SaveChangesAsync();

    return new Response(200,"Feedback criado com sucesso!",new { feedback.Id });
  }
}