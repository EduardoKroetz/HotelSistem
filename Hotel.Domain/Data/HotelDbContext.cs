using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.CategoryEntity;
using Hotel.Domain.Entities.RoomContext.ImageEntity;
using Hotel.Domain.Entities.RoomContext.ReportEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Data;

public class HotelDbContext : DbContext
{
  public DbSet<Admin> Admins { get; set; }
  public DbSet<Permission> Permissions { get; set; }
  public DbSet<Customer> Customers { get; set; }
  public DbSet<Feedback> Feedbacks { get; set; }
  public DbSet<Employee> Employees { get; set; }
  public DbSet<Responsability> Responsabilities { get; set; }
  public DbSet<RoomInvoice> RoomInvoices { get; set; }
  public DbSet<Reservation> Reservations { get; set; }
  public DbSet<Category> Categories { get; set; }
  public DbSet<Image> Images { get; set; }
  public DbSet<Report> Reports { get; set; }
  public DbSet<Service> Services { get; set; }
  public DbSet<Room> Rooms { get; set; }


}