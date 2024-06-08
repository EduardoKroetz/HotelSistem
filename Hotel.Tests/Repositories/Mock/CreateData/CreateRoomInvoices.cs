

using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.Mock.CreateData;

public class CreateRoomInvoices
{
  public static async Task Create()
  {
    var reservationWithService = await BaseRepositoryTest.MockConnection.Context.Reservations.FirstOrDefaultAsync(x => x.Services.Count > 0 && x.Status == EReservationStatus.Pending &&  x.ExpectedCheckIn.Date == DateTime.Now.Date)
      ?? throw new Exception("reserva com serviço não encontrada.");

    reservationWithService.ToCheckIn();

    var roomInvoices = new List<RoomInvoice>()
    {
      reservationWithService.Finish(EPaymentMethod.CreditCard),
      BaseRepositoryTest.ReservationsToFinish[0].Finish(EPaymentMethod.Pix),
      BaseRepositoryTest.ReservationsToFinish[1].Finish(EPaymentMethod.CreditCard,40),
      BaseRepositoryTest.ReservationsToFinish[2].Finish(EPaymentMethod.Pix,10),
    };
    roomInvoices[0].FinishInvoice();

    await BaseRepositoryTest.MockConnection.Context.RoomInvoices.AddRangeAsync(roomInvoices);
    await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

    BaseRepositoryTest.RoomInvoices = await BaseRepositoryTest.MockConnection.Context.RoomInvoices.ToListAsync();
  }
}