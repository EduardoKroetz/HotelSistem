using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.ResponsibilityEntity;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Entities.InvoiceEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.ServiceEntity;

public partial class Service : Entity, IService
{
    internal Service() { }
    public Service(string name, string description, decimal price, EPriority priority, int timeInMinutes, string stripeProductId = "")
    {
        Name = name;
        Description = description;
        Price = price;
        IsActive = true;
        Priority = priority;
        TimeInMinutes = timeInMinutes;
        StripeProductId = stripeProductId;
        Responsibilities = [];

        Validate();
    }

    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; }
    public EPriority Priority { get; private set; }
    public int TimeInMinutes { get; private set; }
    public string StripeProductId { get; set; } = null!;
    public ICollection<Responsibility> Responsibilities { get; private set; } = [];
    public ICollection<Reservation> Reservations { get; private set; } = [];
    public ICollection<Invoice> Invoices { get; private set; } = [];
    public ICollection<Room> Rooms { get; private set; } = [];

}