

using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.Mock.CreateData;

public class CreateRoomInvoices
{
  public static async Task Create()
  {
    var reservationWithService = await BaseRepositoryTest.MockConnection.Context.Reservations.FirstOrDefaultAsync(x => x.Services.Count > 0 && x.Status == EReservationStatus.Pending &&  x.CheckIn.Date == DateTime.Now.Date)
      ?? throw new Exception("reserva com serviço não encontrada.");

    var roomInvoices = new List<RoomInvoice>()
    {
      reservationWithService.GenerateInvoice(EPaymentMethod.CreditCard),
      BaseRepositoryTest.ReservationsToGenerateInvoice[0].GenerateInvoice(EPaymentMethod.Pix),
      BaseRepositoryTest.ReservationsToGenerateInvoice[1].GenerateInvoice(EPaymentMethod.CreditCard,40),
      BaseRepositoryTest.ReservationsToGenerateInvoice[2].GenerateInvoice(EPaymentMethod.Pix,10),
      BaseRepositoryTest.ReservationsToGenerateInvoice[3].GenerateInvoice(EPaymentMethod.Pix,30),
      BaseRepositoryTest.ReservationsToGenerateInvoice[4].GenerateInvoice(EPaymentMethod.CreditCard,16),
    };
    roomInvoices[0].FinishInvoice();

    await BaseRepositoryTest.MockConnection.Context.RoomInvoices.AddRangeAsync(roomInvoices);
    await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

    BaseRepositoryTest.RoomInvoices = await BaseRepositoryTest.MockConnection.Context.RoomInvoices.ToListAsync();
  }
}