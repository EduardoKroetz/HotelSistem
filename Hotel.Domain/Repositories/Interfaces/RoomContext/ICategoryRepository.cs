using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Entities.RoomContext.CategoryEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface ICategoryRepository : IRepository<Category>, IRepositoryQuery<GetCategory>
{
}