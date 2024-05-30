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
  private readonly IFeedbackRepository  _repository;
  private readonly ICustomerRepository _customerRepository;
  private readonly IReservationRepository _reservationRepository;
  private readonly IRoomRepository _roomRepository;

  public FeedbackHandler(IFeedbackRepository repository, ICustomerRepository customerRepository, IReservationRepository reservationRepository, IRoomRepository roomRepository)
  {
    _repository = repository;
    _customerRepository = customerRepository;
    _reservationRepository = reservationRepository;
    _roomRepository = roomRepository;
  }

  public async Task<Response<object>> HandleCreateAsync(CreateFeedback model, Guid userId)
  {
    var customer = await _customerRepository.GetEntityByIdAsync(userId);
    if (customer == null)
      throw new ArgumentException("Usuário não encontrado ou não possui permissão.");

    var reservation = await _reservationRepository.GetReservationIncludesCustomers(model.ReservationId);
    if (reservation == null)
      throw new ArgumentException("Reserva não encontrada.");

    var room = await _roomRepository.GetEntityByIdAsync(model.RoomId);
    if (room == null)
      throw new ArgumentException("Hospedagem não encontrada.");

    var feedback = new Feedback(
      model.Comment,model.Rate,customer.Id,model.ReservationId,model.RoomId, reservation //vai validar se o cliente que está criando o feedback está na reserva
    );

    await _repository.CreateAsync(feedback);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Feedback registrado.",new { feedback.Id });
  }
}