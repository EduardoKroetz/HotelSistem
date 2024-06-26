using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.ServiceDTOs;

public class EditorService : IDataTransferObject
{
    public EditorService(string name, string description, decimal price, EPriority priority, int timeInMinutes)
    {
        Name = name;
        Description = description;
        Price = price;
        Priority = priority;
        TimeInMinutes = timeInMinutes;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public EPriority Priority { get; private set; }
    public int TimeInMinutes { get; private set; }
}

