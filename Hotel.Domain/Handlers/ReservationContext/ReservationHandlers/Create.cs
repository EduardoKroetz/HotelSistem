using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Exceptions;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Handlers.PaymentContext.RoomInvoiceHandlers;
using Hotel.Domain.Repositories.Interfaces.CustomerContext;
using Hotel.Domain.Repositories.Interfaces.ReservationContext;
using Hotel.Domain.Repositories.Interfaces.RoomContext;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler : IHandler
{
  private readonly IReservationRepository  _repository;
  private readonly IRoomRepository  _roomRepository;
  private readonly ICustomerRepository _customerRepository;
  private readonly IServiceRepository _serviceRepository;
  private readonly RoomInvoiceHandler _invoiceHandler;
  public ReservationHandler(IReservationRepository repository, IRoomRepository roomRepository,ICustomerRepository customerRepository, IServiceRepository serviceRepository, RoomInvoiceHandler invoiceHandler)
  {
    _repository = repository;
    _roomRepository = roomRepository;
    _customerRepository = customerRepository;
    _serviceRepository = serviceRepository;
    _invoiceHandler = invoiceHandler;
  }


  public async Task<Response> HandleCreateAsync(CreateReservation model, Guid customerId)
  {
    var room = await _roomRepository.GetEntityByIdAsync(model.RoomId)
      ?? throw new NotFoundException("Hospedagem não encontrada.");

    //Buscar todos os Customers através dos IDs passados
    var customer = await _customerRepository.GetEntityByIdAsync(customerId)
      ?? throw new NotFoundException("Usuário não encontrado.");

    var reservation = new Reservation(room,model.CheckIn,customer, model.Capacity ,model.CheckOut);

    await _repository.CreateAsync(reservation);
    await _repository.SaveChangesAsync();

    return new Response(200,"Reserva criada com sucesso!",new { reservation.Id });
  }
}