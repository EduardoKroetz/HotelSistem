using Hotel.Domain.Entities.InvoiceEntity;
using Hotel.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.UnitTests.Repositories.Mock.CreateData;

public class CreateInvoices
{
    public static async Task Create()
    {
        var reservationWithService = await BaseRepositoryTest.MockConnection.Context.Reservations.FirstOrDefaultAsync(x => x.Services.Count > 0 && x.Status == EReservationStatus.CheckedIn && x.ExpectedCheckIn.Date == DateTime.Now.Date)
          ?? throw new Exception("reserva com serviço não encontrada.");

        var roomInvoices = new List<Invoice>()
        {
          reservationWithService.Finish(),
          BaseRepositoryTest.ReservationsToFinish[0].Finish(),
          BaseRepositoryTest.ReservationsToFinish[1].Finish(),
          BaseRepositoryTest.ReservationsToFinish[2].Finish(),
        };

        await BaseRepositoryTest.MockConnection.Context.Invoices.AddRangeAsync(roomInvoices);
        await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

        BaseRepositoryTest.Invoices = await BaseRepositoryTest.MockConnection.Context.Invoices.ToListAsync();
    }
}