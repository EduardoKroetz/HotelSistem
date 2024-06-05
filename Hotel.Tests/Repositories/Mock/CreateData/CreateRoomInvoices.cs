

using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.Mock.CreateData;

public class CreateRoomInvoices
{
  public static async Task Create()
  {
    var reservationWithService = await BaseRepositoryTest.MockConnection.Context.Reservations.FirstOrDefaultAsync(x => x.Services.Count > 0 && x.Status == EReservationStatus.Pending)
      ?? throw new Exception("reserva com serviço não encontrada.");

    var reservationsWithStatusPending = await BaseRepositoryTest.MockConnection.Context.Reservations.Where(x => x.Status == EReservationStatus.Pending && x.Services.Count == 0).ToListAsync();
    var roomInvoices = new List<RoomInvoice>()
    {
      reservationWithService.GenerateInvoice(EPaymentMethod.CreditCard),
      reservationsWithStatusPending[0].GenerateInvoice(EPaymentMethod.Pix),
      reservationsWithStatusPending[1].GenerateInvoice(EPaymentMethod.CreditCard,40),
      reservationsWithStatusPending[2].GenerateInvoice(EPaymentMethod.Pix,10),
      reservationsWithStatusPending[3].GenerateInvoice(EPaymentMethod.Pix,30),
      reservationsWithStatusPending[4].GenerateInvoice(EPaymentMethod.CreditCard,16),
    };
    roomInvoices[0].FinishInvoice();

    await BaseRepositoryTest.MockConnection.Context.RoomInvoices.AddRangeAsync(roomInvoices);
    await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

    BaseRepositoryTest.RoomInvoices = await BaseRepositoryTest.MockConnection.Context.RoomInvoices.ToListAsync();
  }
}