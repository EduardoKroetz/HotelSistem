using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities.CustomerContext;

public partial class Customer : User, ICustomer
{
  public Customer(Name name, Email email, Phone phone, string password, EGender? gender = null, DateTime? dateOfBirth = null, Address? address = null)
    : base(name,email,phone,password,gender,dateOfBirth,address)
  {
    Feedbacks = [];
  }

  public HashSet<Feedback> Feedbacks { get; private set; }
}