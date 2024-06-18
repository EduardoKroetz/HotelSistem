using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.FeedbackEntity;
using Hotel.Domain.Entities.EmployeeEntity;
using Hotel.Domain.Entities.ResponsibilityEntity;
using Hotel.Domain.Entities.ImageEntity;
using Hotel.Domain.Entities.PermissionEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Tests.UnitTests.Entities;

public static class TestParameters
{
    public static readonly Name Name = new("Bruce", "Wayne");
    public static readonly Email Email = new("batman@gmail.com");
    public static readonly Phone Phone = new("+55 (55) 99255-3344");
    public static readonly Address Address = new("Brazil", "Gotham", "Batman street", 999);
    public static readonly Permission Permission = new("Criar administrador", "Criar administrador no sistema");
    public static readonly string Password = "batmanpassword123";
    public static readonly Category Category = new("Quarto básico", "Quarto básico para hospedagem diária.", 45m);
    public static readonly Room Room = new(22, 50m, 3, "Um quarto para hospedagem.", Category.Id);
    public static readonly Customer Customer = new(Name, Email, Phone, Password, EGender.Masculine, DateTime.Now.AddYears(-18), Address);
    public static readonly Reservation Reservation = new(Room, DateTime.Now.AddDays(3), DateTime.Now.AddDays(4), Customer, 2);
    public static readonly Feedback Feedback = new("Muito bom.", 10, Customer.Id, Reservation.Id, Room.Id);
    public static readonly Admin Admin = new(Name, Email, Phone, Password, EGender.Masculine, DateTime.Now.AddYears(-18), Address);
    public static readonly Responsibility Responsibility = new("Responder serviços", "Responder serviços de quarto", EPriority.Medium);
    public static readonly Employee Employee = new(Name, Email, Phone, Password, EGender.Masculine, DateTime.Now.AddYears(-18), Address, 1500m);
    public static readonly Service Service = new("Servico de quarto", 30m, EPriority.Medium, 30);
    public static readonly Image Image = new("http://:", Room.Id);
    public const string DescriptionMaxCaracteres = "sagittis vitae et leo duis ut diam quam nulla porttitor massa id neque aliquam vestibulum morbi blandit cursus risus at ultrices mi tempus imperdiet nulla malesuada pellentesque elit eget gravida cum sociis natoque penatibus et magnis dis parturient montes nascetur ridiculus mus mauris vitae ultricies leo integer malesuada nunc vel risus commodo viverra maecenas accumsan lacus vel facilisis volutpat est velit egestas dui id ornare arcu odio ut sem nulla pharetra diam sit amet nisl suscipit adipia";
}