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

  public  const string DescriptionMaxCaracteres = "sagittis vitae et leo duis ut diam quam nulla porttitor massa id neque aliquam vestibulum morbi blandit cursus risus at ultrices mi tempus imperdiet nulla malesuada pellentesque elit eget gravida cum sociis natoque penatibus et magnis dis parturient montes nascetur ridiculus mus mauris vitae ultricies leo integer malesuada nunc vel risus commodo viverra maecenas accumsan lacus vel facilisis volutpat est velit egestas dui id ornare arcu odio ut sem nulla pharetra diam sit amet nisl suscipit adipia";
}