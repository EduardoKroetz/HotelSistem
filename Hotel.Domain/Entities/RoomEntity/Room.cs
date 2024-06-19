using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.ImageEntity;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.RoomEntity;

public partial class Room : Entity, IRoom
{
    internal Room() { }
    public Room(string name,int number, decimal price, int capacity, string description, Category category, string stripeProductId = "")
    {
        Name = name;
        Number = number;
        Price = price;
        Capacity = capacity;
        Description = description;
        IsActive = true;
        CategoryId = category.Id;
        Category = category;
        StripeProductId = stripeProductId;  
        Status = ERoomStatus.Available;
        Services = [];
        Images = [];

        Validate();
    }

    public string Name { get; private set; } = null!;
    public int Number { get; private set; }
    public decimal Price { get; private set; }
    public ERoomStatus Status { get; private set; }
    public int Capacity { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public string StripeProductId { get; private set; } = null!;
    public ICollection<Service> Services { get; private set; } = [];
    public Guid CategoryId { get; private set; }
    public Category? Category { get; private set; }
    public ICollection<Image> Images { get; private set; } = [];
    public ICollection<Reservation> Reservations { get; private set; } = [];
}