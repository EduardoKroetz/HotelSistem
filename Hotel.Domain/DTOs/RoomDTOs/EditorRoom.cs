namespace Hotel.Domain.DTOs.RoomDTOs;

public class EditorRoom : IDataTransferObject
{
    public EditorRoom(string name ,int number, decimal price, int capacity, string description, Guid categoryId)
    {
        Name = name;
        Number = number;
        Price = price;
        Capacity = capacity;
        Description = description;
        CategoryId = categoryId;
    }

    public string Name { get; private set; }
    public int Number { get; private set; }
    public decimal Price { get; private set; }
    public int Capacity { get; private set; }
    public string Description { get; private set; }
    public Guid CategoryId { get; private set; }
}

