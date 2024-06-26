using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ReservationDTOs;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Exceptions;
using Hotel.Domain.Handlers.Base.Interfaces;
using Hotel.Domain.Handlers.InvoiceHandlers;
using Hotel.Domain.Repositories.Interfaces;
using Hotel.Domain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Hotel.Domain.Handlers.ReservationHandlers;

public partial class ReservationHandler : IHandler
{
    private readonly IReservationRepository _repository;
    private readonly IRoomRepository _roomRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly InvoiceHandler _invoiceHandler;
    private readonly IStripeService _stripeService;

    public ReservationHandler(IReservationRepository repository, IRoomRepository roomRepository, ICustomerRepository customerRepository, IServiceRepository serviceRepository, InvoiceHandler invoiceHandler, IStripeService stripeService)
    {
        _repository = repository;
        _roomRepository = roomRepository;
        _customerRepository = customerRepository;
        _serviceRepository = serviceRepository;
        _invoiceHandler = invoiceHandler;
        _stripeService = stripeService;
    }

    public async Task<Response> HandleCreateAsync(CreateReservation model, Guid customerId)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var room = await _roomRepository.GetEntityByIdAsync(model.RoomId)
                ?? throw new NotFoundException("Hospedagem não encontrada");

            var customer = await _customerRepository.GetEntityByIdAsync(customerId)
                ?? throw new NotFoundException("Usuário não encontrado");

            var reservation = new Reservation(room, model.ExpectedCheckIn, model.ExpectedCheckOut, customer, model.Capacity);

            try
            {
                await _repository.CreateAsync(reservation);
                await _repository.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                throw new DbUpdateException("Ocorreu um erro ao criar a reserva no banco de dados");
            }

            try
            {
                var paymentIntent = await _stripeService.CreatePaymentIntentAsync
                (
                    reservation.ExpectedTotalAmount(), 
                    customer.StripeCustomerId, 
                    room
                );
                reservation.StripePaymentIntentId = paymentIntent.Id;
                await _repository.SaveChangesAsync();
            }
            catch (StripeException e)
            {
                throw new StripeException($"Ocorreu um erro ao lidar com o serviço de pagamento. Erro: {e.Message}");
            }

            await transaction.CommitAsync();

            return new Response("Reserva criada com sucesso!", new { reservation.Id, reservation.StripePaymentIntentId });
        }
        catch 
        {
            await transaction.RollbackAsync();
            throw;
        }

      
    }
}