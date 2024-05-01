using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities.CustomerContext;

public partial class Customer : User
{
  public Customer(Name name, Email email, Phone phone, string password, EGender? gender, DateTime? dateOfBirth, Address? address) 
    : base(name,email,phone,password,gender,dateOfBirth,address)
  {
    Feedbacks = [];
  }

  public List<Feedback> Feedbacks { get; private set; }
}