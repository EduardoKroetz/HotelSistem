using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.CustomerContext.FeedbackContext;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities.CustomerContext;

public partial class Customer : User, ICustomer
{
  internal Customer(){}
  
  public Customer(Name name, Email email, Phone phone, string password, EGender? gender = null, DateTime? dateOfBirth = null, Address? address = null)
    : base(name,email,phone,password,gender,dateOfBirth,address)
  {
  }

  public HashSet<Feedback> Feedbacks { get; private set; } = [];
  public HashSet<Reservation> Reservations { get; private set; } = [];
  public HashSet<RoomInvoice> RoomInvoices { get; private set; } = [];
  public ICollection<Like> Likes { get; private set; } = [];
  public ICollection<Deslike> Dislikes { get; private set; } = [];
}