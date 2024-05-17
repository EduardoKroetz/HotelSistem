using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.Mock.CreateData;

public class CreateFeedbacks
{
  public static async Task Create()
  {
    var feedbacks = new List<Feedback>()
    {
      new("O serviço do quarto estava ótimo", 6, BaseRepositoryTest.Customers[0].Id, BaseRepositoryTest.Reservations[0].Id, BaseRepositoryTest.Rooms[0].Id),
      new("Excelente atendimento na recepção!", 5, BaseRepositoryTest.Customers[1].Id, BaseRepositoryTest.Reservations[1].Id, BaseRepositoryTest.Rooms[1].Id),
      new("A limpeza do quarto deixou a desejar", 3, BaseRepositoryTest.Customers[2].Id, BaseRepositoryTest.Reservations[2].Id, BaseRepositoryTest.Rooms[2].Id),
      new("O café da manhã estava delicioso", 4, BaseRepositoryTest.Customers[3].Id, BaseRepositoryTest.Reservations[3].Id, BaseRepositoryTest.Rooms[3].Id),
      new("Problemas com o Wi-Fi, muito lento", 2, BaseRepositoryTest.Customers[4].Id, BaseRepositoryTest.Reservations[4].Id, BaseRepositoryTest.Rooms[4].Id),
    };

    for (var i = 0; i < 6; i++)
    {
      feedbacks[0].AddLike();
      feedbacks[0].AddDeslike();
      feedbacks[4].AddLike();
      feedbacks[2].AddDeslike();
    }

    await BaseRepositoryTest.MockConnection.Context.Feedbacks.AddRangeAsync(feedbacks);
    await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

    BaseRepositoryTest.Feedbacks = await BaseRepositoryTest.MockConnection.Context.Feedbacks.ToListAsync();
  }
}

