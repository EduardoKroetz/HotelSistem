using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ReservationDTOs;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Exceptions;
using Hotel.Domain.Handlers.Base.Interfaces;
using Hotel.Domain.Handlers.InvoiceHandlers;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Handlers.ReservationHandlers;

public partial class ReservationHandler : IHandler
{
    private readonly IReservationRepository _repository;
    private readonly IRoomRepository _roomRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly InvoiceHandler _invoiceHandler;
    public ReservationHandler(IReservationRepository repository, IRoomRepository roomRepository, ICustomerRepository customerRepository, IServiceRepository serviceRepository, InvoiceHandler invoiceHandler)
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

        var reservation = new Reservation(room, model.ExpectedCheckIn, model.ExpectedCheckOut, customer, model.Capacity);

        await _repository.CreateAsync(reservation);
        await _repository.SaveChangesAsync();

        return new Response("Reserva criada com sucesso!", new { reservation.Id });
    }
}