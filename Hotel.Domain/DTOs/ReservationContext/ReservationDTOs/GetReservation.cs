using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;

public class GetReservation : IDataTransferObject
{
  public GetReservation(Guid id, decimal dailyRate, int? hostedDays, DateTime checkIn, DateTime? checkOut, EReservationStatus status, int capacity, Guid roomId, ICollection<GetUser> customers, Guid? invoiceId, ICollection<Service> services)
  {
    Id = id;
    DailyRate = dailyRate;
    HostedDays = hostedDays;
    CheckIn = checkIn;
    CheckOut = checkOut;
    Status = status;
    Capacity = capacity;
    RoomId = roomId;
    Customers = customers;
    InvoiceId = invoiceId;
    Services = services;
  }
  public Guid Id { get; private set; }
  public decimal DailyRate { get; private set; }
  public int? HostedDays { get; private set; }
  public DateTime CheckIn { get; private set; }
  public DateTime? CheckOut { get; private set; }
  public EReservationStatus Status { get; private set; }
  public int Capacity { get; private set; }
  public Guid RoomId { get; private set; }
  public ICollection<GetUser> Customers { get; private set; }
  public Guid? InvoiceId { get; private set; }
  public ICollection<Service> Services { get; private set; }
}