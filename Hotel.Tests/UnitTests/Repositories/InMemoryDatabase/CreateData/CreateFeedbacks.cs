using Hotel.Domain.Entities.FeedbackEntity;
using Hotel.Domain.Entities.DislikeEntity;
using Hotel.Domain.Entities.LikeEntity;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.UnitTests.Repositories.Mock.CreateData;

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

        var likes = new List<Like>()
        {
            new (BaseRepositoryTest.Customers[0], feedbacks[0]),
            new (BaseRepositoryTest.Customers[1], feedbacks[1]),
            new (BaseRepositoryTest.Customers[2], feedbacks[2]),
            new (BaseRepositoryTest.Customers[3], feedbacks[3]),
            new (BaseRepositoryTest.Customers[4], feedbacks[4]),
            new (BaseRepositoryTest.Customers[3], feedbacks[0]),
            new (BaseRepositoryTest.Customers[2], feedbacks[2]),
            new (BaseRepositoryTest.Customers[1], feedbacks[0]),
            new (BaseRepositoryTest.Customers[0], feedbacks[4]),
        };

        var deslikes = new List<Dislike>()
        {
            new (BaseRepositoryTest.Customers[0], feedbacks[0]),
            new (BaseRepositoryTest.Customers[1], feedbacks[1]),
            new (BaseRepositoryTest.Customers[2], feedbacks[2]),
            new (BaseRepositoryTest.Customers[3], feedbacks[3]),
            new (BaseRepositoryTest.Customers[4], feedbacks[4]),
            new (BaseRepositoryTest.Customers[3], feedbacks[0]),
            new (BaseRepositoryTest.Customers[2], feedbacks[2]),
            new (BaseRepositoryTest.Customers[1], feedbacks[0]),
            new (BaseRepositoryTest.Customers[0], feedbacks[4]),
        };

        await BaseRepositoryTest.MockConnection.Context.Likes.AddRangeAsync(likes);
        await BaseRepositoryTest.MockConnection.Context.Dislikes.AddRangeAsync(deslikes);

        await BaseRepositoryTest.MockConnection.Context.Feedbacks.AddRangeAsync(feedbacks);
        await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

        BaseRepositoryTest.Feedbacks = await BaseRepositoryTest.MockConnection.Context.Feedbacks.ToListAsync();
    }
}

