using Hotel.Domain.Entities.DislikeEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IDeslikeRepository
{
    Task<Dislike?> GetDeslikeAsync(Guid feedbackId, Guid customerId);
    void RemoveDeslike(Dislike dislike);
    Task CreateDeslike(Dislike dislike);
}
