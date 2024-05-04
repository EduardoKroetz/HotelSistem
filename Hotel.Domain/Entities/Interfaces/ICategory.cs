using Hotel.Domain.Entities.Base.Interfaces;

namespace Hotel.Domain.Entities.Interfaces;

public interface ICategory : IEntity
{
    string Name { get; }
    string Description { get; }
    decimal AveragePrice { get; }
}
