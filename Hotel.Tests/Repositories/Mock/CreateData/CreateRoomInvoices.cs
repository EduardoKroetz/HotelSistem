

using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.Mock.CreateData;

public class CreateRoomInvoices
{
  public static async Task Create()
  {
    var roomInvoices = new List<RoomInvoice>()
    {
      BaseRepositoryTest.Reservations[0].GenerateInvoice(EPaymentMethod.Pix),
      BaseRepositoryTest.Reservations[1].GenerateInvoice(EPaymentMethod.CreditCard,40),
      BaseRepositoryTest.Reservations[2].GenerateInvoice(EPaymentMethod.Pix,10),
      BaseRepositoryTest.Reservations[3].GenerateInvoice(EPaymentMethod.Pix,30),
      BaseRepositoryTest.Reservations[4].GenerateInvoice(EPaymentMethod.CreditCard,16),
    };
    roomInvoices[0].FinishInvoice();

    await BaseRepositoryTest.MockConnection.Context.RoomInvoices.AddRangeAsync(roomInvoices);
    await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

    BaseRepositoryTest.RoomInvoices = await BaseRepositoryTest.MockConnection.Context.RoomInvoices.ToListAsync();
  }
}