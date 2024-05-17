

using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.Mock.CreateData;

public class CreateReservations
{
  public static async Task Create()
  {
    var reservations = new List<Reservation>()
    {
      new(BaseRepositoryTest.Rooms[0], DateTime.Now, [BaseRepositoryTest.Customers[0]],DateTime.Now.AddDays(2)),
      new(BaseRepositoryTest.Rooms[1], DateTime.Now, [BaseRepositoryTest.Customers[1]],DateTime.Now.AddDays(8)),
      new(BaseRepositoryTest.Rooms[2], DateTime.Now, [BaseRepositoryTest.Customers[2]],DateTime.Now.AddDays(5)),
      new(BaseRepositoryTest.Rooms[3], DateTime.Now, [BaseRepositoryTest.Customers[3]],DateTime.Now.AddDays(8)),
      new(BaseRepositoryTest.Rooms[4], DateTime.Now, [BaseRepositoryTest.Customers[4]],DateTime.Now.AddDays(5)),
      new(BaseRepositoryTest.Rooms[5], DateTime.Now, [BaseRepositoryTest.Customers[5]],DateTime.Now.AddDays(4)),
      new(BaseRepositoryTest.Rooms[6], DateTime.Now, [BaseRepositoryTest.Customers[4]],DateTime.Now.AddDays(2)),
      new(BaseRepositoryTest.Rooms[7], DateTime.Now, [BaseRepositoryTest.Customers[3]],DateTime.Now.AddDays(3)),
      new(BaseRepositoryTest.Rooms[8], DateTime.Now, [BaseRepositoryTest.Customers[2]],DateTime.Now.AddDays(9)),
      new(BaseRepositoryTest.Rooms[9], DateTime.Now, [BaseRepositoryTest.Customers[1]],DateTime.Now.AddDays(1)),
    };

    reservations[0].AddService(BaseRepositoryTest.Services[0]);
    reservations[1].AddService(BaseRepositoryTest.Services[1]);
    reservations[2].AddService(BaseRepositoryTest.Services[2]);
    reservations[3].AddService(BaseRepositoryTest.Services[3]);
    reservations[4].AddService(BaseRepositoryTest.Services[4]);

    await BaseRepositoryTest.MockConnection.Context.Reservations.AddRangeAsync(reservations);
    await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

    BaseRepositoryTest.Reservations = await BaseRepositoryTest.MockConnection.Context.Reservations.ToListAsync();
  }
}
