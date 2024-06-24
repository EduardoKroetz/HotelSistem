using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Handlers.ReservationHandlers;

public partial class ReservationHandler
{
    public async Task<Response> HandleAddServiceAsync(Guid id, Guid serviceId)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var reservation = await _repository.GetReservationIncludesServices(id)
                ?? throw new NotFoundException("Reserva não encontrada.");

            var service = await _serviceRepository.GetEntityByIdAsync(serviceId)
                ?? throw new NotFoundException("Serviço não encontrado.");

            var room = await _roomRepository.GetRoomIncludesServices(reservation.RoomId)
                ?? throw new NotFoundException("A hospedagem relacionada a reserva não foi encontrado.");

            if (room.Services.Contains(service) is false)
                throw new ArgumentException("Esse serviço não está dísponível nessa hospedagem.");

            reservation.AddService(service);

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Ocorreu um erro ao salvar o serviço no banco de dados");
            }

            await _stripeService.AddPaymentIntentProduct(reservation.StripePaymentIntentId, service);
            
            await transaction.CommitAsync();

            return new Response("Serviço adicionado com sucesso!");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}