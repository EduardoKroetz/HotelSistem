﻿
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.Mock.CreateData;

public class CreateRooms
{
  public static async Task Create()
  {
    var rooms = new List<Room>()
    {
      new(4, 50m, 3, "Um quarto para hospedagem.", BaseRepositoryTest.Categories[1].Id),
      new(3, 110m, 4, "Um quarto com vista para a praia.", BaseRepositoryTest.Categories[0].Id),
      new(9, 50m, 3, "Quarto Standard", BaseRepositoryTest.Categories[1].Id),
      new(11, 90m, 2, "Suíte Deluxe", BaseRepositoryTest.Categories[0].Id),
      new(13, 110m, 2, "Suíte Executiva", BaseRepositoryTest.Categories[2].Id),
      new(29, 70m, 4, "Quarto Familiar", BaseRepositoryTest.Categories[3].Id),
      new(14, 150m, 2, "Suíte Presidencial", BaseRepositoryTest.Categories[4].Id),
      new(12, 80m, 2, "Quarto Standard", BaseRepositoryTest.Categories[1].Id),
      new(21, 120m, 3, "Suíte Familiar", BaseRepositoryTest.Categories[3].Id),
      new(25, 200m, 2, "Suíte Real", BaseRepositoryTest.Categories[2].Id),
      new(19, 100m, 2, "Quarto Deluxe", BaseRepositoryTest.Categories[0].Id),
      new(31, 180m, 2, "Suíte Presidencial", BaseRepositoryTest.Categories[4].Id)
    };

    new Reservation(rooms[0], DateTime.Now.AddDays(1), [BaseRepositoryTest.Customers[0]]);

    await BaseRepositoryTest.MockConnection.Context.Rooms.AddRangeAsync(rooms);
    await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

    BaseRepositoryTest.Rooms = await BaseRepositoryTest.MockConnection.Context.Rooms.ToListAsync();

    BaseRepositoryTest.Rooms[0].AddService(BaseRepositoryTest.Services[0]);
    BaseRepositoryTest.Rooms[0].AddService(BaseRepositoryTest.Services[2]);
    BaseRepositoryTest.Rooms[1].AddService(BaseRepositoryTest.Services[4]);
    BaseRepositoryTest.Rooms[3].AddService(BaseRepositoryTest.Services[1]);
    BaseRepositoryTest.Rooms[4].AddService(BaseRepositoryTest.Services[3]);
    BaseRepositoryTest.Rooms[4].AddService(BaseRepositoryTest.Services[0]);
    BaseRepositoryTest.Rooms[6].AddService(BaseRepositoryTest.Services[2]);
    BaseRepositoryTest.Rooms[8].AddService(BaseRepositoryTest.Services[4]);
    BaseRepositoryTest.Rooms[10].AddService(BaseRepositoryTest.Services[1]);
    BaseRepositoryTest.Rooms[11].AddService(BaseRepositoryTest.Services[3]);
    BaseRepositoryTest.Rooms[11].AddService(BaseRepositoryTest.Services[0]);
    BaseRepositoryTest.Rooms[1].AddService(BaseRepositoryTest.Services[2]);
    BaseRepositoryTest.Rooms[5].AddService(BaseRepositoryTest.Services[3]);
    BaseRepositoryTest.Rooms[7].AddService(BaseRepositoryTest.Services[1]);
    BaseRepositoryTest.Rooms[9].AddService(BaseRepositoryTest.Services[0]);

    await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();
    BaseRepositoryTest.Rooms = await BaseRepositoryTest.MockConnection.Context.Rooms.ToListAsync();
  }
}