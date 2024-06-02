using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces.PaymentContext;
using Hotel.Domain.Repositories.Interfaces.ReservationContext;


namespace Hotel.Domain.Handlers.PaymentContext.RoomInvoiceHandlers;

public partial class RoomInvoiceHandler : IHandler
{
  private readonly IRoomInvoiceRepository  _repository;
  private readonly IReservationRepository  _reservationRepository;
  public RoomInvoiceHandler(IRoomInvoiceRepository repository, IReservationRepository reservationRepository)
  {
    _repository = repository;
    _reservationRepository = reservationRepository;
  }
  

  public async Task<Response> HandleCreateAsync(CreateRoomInvoice model)
  {
    var reservation = await _reservationRepository.GetReservationIncludesCustomers(model.ReservationId);
    if (reservation == null)
      throw new ArgumentException("Reserva não encontrada.");

    var roomInvoice = reservation.GenerateInvoice(model.PaymentMethod, model.TaxInformation);

    await _repository.CreateAsync(roomInvoice);
    await _repository.SaveChangesAsync();

    return new Response(200,"Fatura de quarto criada com sucesso!",new { roomInvoice.Id });
  }
}