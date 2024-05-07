using System.Collections.ObjectModel;
using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler : IHandler
{
  private readonly IReservationRepository  _repository;
  private readonly IRoomRepository  _roomRepository;
  private readonly ICustomerRepository  _customerRepository;
  public ReservationHandler(IReservationRepository repository, IRoomRepository roomRepository,ICustomerRepository customerRepository)
  {
    _repository = repository;
    _roomRepository = roomRepository;
    _customerRepository = customerRepository;
  }


  public async Task<Response<object>> HandleCreateAsync(CreateReservation model)
  {
    var room = await _roomRepository.GetEntityByIdAsync(model.RoomId);
    if (room == null)
      throw new ArgumentException("Quarto não encontrado.");

    //Buscar todos os Customers através dos IDs passados
    var customers = new List<Customer>(
      await _customerRepository.GetCustomersByListId(model.Customers)
    );

    var reservation = new Reservation(room,model.CheckIn,customers,model.CheckOut);

    await _repository.CreateAsync(reservation);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Reserva criada.",new { reservation.Id });
  }
}