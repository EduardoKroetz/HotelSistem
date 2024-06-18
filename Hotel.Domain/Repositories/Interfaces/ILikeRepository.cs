using Hotel.Domain.Entities.LikeEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface ILikeRepository
{
    Task<Like?> GetLikeAsync(Guid feedbackId, Guid customerId);
    void RemoveLike(Like like);
    Task CreateLike(Like like);
}
