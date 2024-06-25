using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.PaymentDTOs;
using Hotel.Domain.Exceptions;
using Stripe;

namespace Hotel.Domain.Handlers.ReservationHandlers;

public partial class ReservationHandler
{
    public async Task<Response> HandleMakeReservationCheckInAsync(Guid id, CardOptions cardOptions)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var reservation = await _repository.GetReservationIncludesAll(id)
                ?? throw new NotFoundException("Reserva não encontrada.");

            reservation.ToCheckIn();

            try
            {
                var paymentMethod = await _stripeService.CreatePaymentMethodAsync
                (
                    cardOptions.Number, 
                    cardOptions.ExpMonth, 
                    cardOptions.ExpYear,
                    cardOptions.Cvc
                );

                await _stripeService.AddPaymentMethodToPaymentIntent(reservation.StripePaymentIntentId, paymentMethod.Id);
            }
            catch (StripeException)
            {
                throw new StripeException("Ocorreu um erro ao atualizar o método de pagamento");
            }

            await transaction.CommitAsync();

            return new Response("Check-In realizado com sucesso!");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}