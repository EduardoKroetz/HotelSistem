

using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.Mock.CreateData;

public class CreateReservations
{
  public static async Task Create()
  {
    var rooms = BaseRepositoryTest.Rooms.OrderBy(x => x.Number).ToList();
    var reservations = new List<Reservation>()
    {
      new(rooms[0], DateTime.Now, DateTime.Now.AddDays(2), BaseRepositoryTest.Customers[0], 1),
      new(rooms[1], DateTime.Now.AddDays(3), DateTime.Now.AddDays(8), BaseRepositoryTest.Customers[1], 1),
      new(rooms[2], DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), BaseRepositoryTest.Customers[2], 1),
      new(rooms[3], DateTime.Now.AddDays(5), DateTime.Now.AddDays(8), BaseRepositoryTest.Customers[3], 1),
      new(rooms[4], DateTime.Now.AddDays(2), DateTime.Now.AddDays(5), BaseRepositoryTest.Customers[4], 1),
      new(rooms[5], DateTime.Now, DateTime.Now.AddDays(4), BaseRepositoryTest.Customers[5], 1),
      new(rooms[6], DateTime.Now, DateTime.Now.AddDays(2), BaseRepositoryTest.Customers[4], 1),
      new(rooms[7], DateTime.Now.AddDays(2), DateTime.Now.AddDays(3), BaseRepositoryTest.Customers[3], 1),
      new(rooms[8], DateTime.Now.AddDays(8), DateTime.Now.AddDays(9), BaseRepositoryTest.Customers[2], 1),
    };



    BaseRepositoryTest.ReservationsToFinish = new List<Reservation>()
    {
      new(rooms[9], DateTime.Now, DateTime.Now.AddDays(1), BaseRepositoryTest.Customers[1], 2),
      new(rooms[10], DateTime.Now, DateTime.Now.AddDays(4), BaseRepositoryTest.Customers[3], 1),
      new(rooms[11], DateTime.Now, DateTime.Now.AddDays(2), BaseRepositoryTest.Customers[2], 1),
    };
    //Fazendo checkIn nas reservas pois não é possível finalizar uma reserva com o CheckIn sendo null
    BaseRepositoryTest.ReservationsToFinish[0].ToCheckIn();
    BaseRepositoryTest.ReservationsToFinish[1].ToCheckIn();
    BaseRepositoryTest.ReservationsToFinish[2].ToCheckIn();


    reservations[0].AddService(BaseRepositoryTest.Services[0]);
    reservations[1].AddService(BaseRepositoryTest.Services[1]);
    reservations[2].AddService(BaseRepositoryTest.Services[2]);
    reservations[3].AddService(BaseRepositoryTest.Services[3]);
    reservations[4].AddService(BaseRepositoryTest.Services[4]);

    await BaseRepositoryTest.MockConnection.Context.Reservations.AddRangeAsync(
      reservations.Concat(BaseRepositoryTest.ReservationsToFinish));
    await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

    BaseRepositoryTest.Reservations = await BaseRepositoryTest.MockConnection.Context.Reservations.ToListAsync();
  }
}
