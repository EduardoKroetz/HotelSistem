﻿using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Hotel.Domain.Handlers.ReservationHandlers;

public partial class ReservationHandler
{
    public async Task<Response> HandleCancelReservationAsync(Guid id, Guid customerId)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var reservation = await _repository.GetReservationIncludesAll(id)
              ?? throw new NotFoundException("Reserva não encontrada.");

            if (reservation.CustomerId != customerId)
                throw new UnauthorizedAccessException("Você não tem permissão para cancelar reserva alheia.");

            reservation.ToCancelled();

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Ocorreu um erro ao atualizar a reserva no banco de dados");
            }

            try
            {
                await _stripeService.CancelPaymentIntentAsync(reservation.StripePaymentIntentId);
            }
            catch (StripeException)
            {
                throw new StripeException("Ocorreu um erro ao cancelar o PaymentIntent no Stripe");
            }

            await transaction.CommitAsync();

            return new Response("Reserva cancelada com sucesso!");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }
}