using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Entities.RoomEntity;

namespace Hotel.Domain.Entities.CategoryEntity;

public partial class Category : Entity, ICategory
{
    internal Category() { }
    public Category(string name, string description, decimal averagePrice)
    {
        Name = name;
        Description = description;
        AveragePrice = averagePrice;

        Validate();
    }

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal AveragePrice { get; private set; }
    public ICollection<Room> Rooms { get; private set; } = [];
}