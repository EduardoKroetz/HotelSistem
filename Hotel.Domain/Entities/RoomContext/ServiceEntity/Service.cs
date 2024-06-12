using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.RoomContext.ServiceEntity;

public partial class Service : Entity, IService
{
  internal Service(){}
  public Service(string name, decimal price, EPriority priority, int timeInMinutes)
  {
    Name = name;
    Price = price;
    IsActive = true;
    Priority = priority;
    TimeInMinutes = timeInMinutes;
    Responsibilities = [];

    Validate();
  }

  public string Name { get; private set; } = string.Empty;
  public decimal Price { get; private set; }
  public bool IsActive { get; private set; }
  public EPriority Priority { get; private set; }
  public int TimeInMinutes { get; private set; }
  public ICollection<Responsibility> Responsibilities { get; private set; } = [];
  public ICollection<Reservation> Reservations { get; private set; }  = [];
  public ICollection<RoomInvoice> RoomInvoices { get; private set; } = [];
  public ICollection<Room> Rooms { get; private set; } = [];

}