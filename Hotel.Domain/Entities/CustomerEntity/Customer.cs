using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.FeedbackEntity;
using Hotel.Domain.Entities.DislikeEntity;
using Hotel.Domain.Entities.InvoiceEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;
using Hotel.Domain.Entities.LikeEntity;
using Hotel.Domain.Entities.Interfaces;

namespace Hotel.Domain.Entities.CustomerEntity;

public partial class Customer : User, ICustomer
{
    internal Customer() { }

    public Customer(Name name, Email email, Phone phone, string password, EGender? gender = null, DateTime? dateOfBirth = null, Address? address = null)
      : base(name, email, phone, password, gender, dateOfBirth, address)
    {
    }

    public string StripeCustomerId { get; set; } = "";
    public ICollection<Feedback> Feedbacks { get; private set; } = [];
    public ICollection<Reservation> Reservations { get; private set; } = [];
    public ICollection<Invoice> Invoices { get; private set; } = [];
    public ICollection<Like> Likes { get; private set; } = [];
    public ICollection<Dislike> Dislikes { get; private set; } = [];
}