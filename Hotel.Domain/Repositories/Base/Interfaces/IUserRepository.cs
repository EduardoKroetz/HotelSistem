using Hotel.Domain.Entities.Base;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Repositories.Base.Interfaces;

public interface IUserRepository<T> : IRepository<T> where T : User 
{
}