using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Tests.Entities;

public static class TestParameters
{
  public static readonly Name Name = new("Bruce","Wayne");
  public static readonly Email Email = new("batman@gmail.com");
  public static readonly Phone Phone = new("+55 (55) 99255-3344");
  public static readonly Address Address = new("Brazil","Gotham","Batman street",999);
  public static readonly string Password = "batmanpassword123";

  public static readonly Admin Admin = new(Name,Email,Phone,Password,EGender.Masculine,DateTime.Now.AddYears(-18),Address);
}