using Hotel.Domain.Data.Mappings.AdminContext;
using Hotel.Domain.Data.Mappings.CustomerContext;
using Hotel.Domain.Data.Mappings.EmployeeContext;
using Hotel.Domain.Data.Mappings.PaymentContext;
using Hotel.Domain.Data.Mappings.ReservationContext;
using Hotel.Domain.Data.Mappings.RoomContext;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.CategoryEntity;
using Hotel.Domain.Entities.RoomContext.ImageEntity;
using Hotel.Domain.Entities.RoomContext.ReportEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Hotel.Domain.Data.Mappings;
using Hotel.Domain.Entities.CustomerContext.FeedbackContext;

namespace Hotel.Domain.Data;

public class HotelDbContext : DbContext
{
  public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
  {
  }

  public DbSet<Admin> Admins { get; set; }
  public DbSet<Permission> Permissions { get; set; }
  public DbSet<Customer> Customers { get; set; }
  public DbSet<Feedback> Feedbacks { get; set; }
  public DbSet<Employee> Employees { get; set; }
  public DbSet<Responsibility> Responsibilities { get; set; }
  public DbSet<RoomInvoice> RoomInvoices { get; set; }
  public DbSet<Reservation> Reservations { get; set; }
  public DbSet<Category> Categories { get; set; }
  public DbSet<Image> Images { get; set; }
  public DbSet<Report> Reports { get; set; }
  public DbSet<Service> Services { get; set; }
  public DbSet<Room> Rooms { get; set; }
  public DbSet<VerificationCode> VerificationCodes { get; set; }
  public DbSet<Like> Likes { get; set; }
  public DbSet<Deslike> Dislikes { get; set; }

  protected override void OnModelCreating(ModelBuilder model)
  {

    model.ApplyConfiguration(new AdminMapping());
    model.ApplyConfiguration(new PermissionMapping());
    model.ApplyConfiguration(new CustomerMapping());
    model.ApplyConfiguration(new FeedbackMapping());
    model.ApplyConfiguration(new EmployeeMapping());
    model.ApplyConfiguration(new ResponsibilityMapping());
    model.ApplyConfiguration(new RoomInvoiceMapping());
    model.ApplyConfiguration(new ReservationMapping());
    model.ApplyConfiguration(new CategoryMapping());
    model.ApplyConfiguration(new ImageMapping());
    model.ApplyConfiguration(new ReportMapping());
    model.ApplyConfiguration(new RoomMapping());
    model.ApplyConfiguration(new ServiceMapping());
    model.ApplyConfiguration(new VerificationCodeMapping());
    model.ApplyConfiguration(new LikeMapping());
    model.ApplyConfiguration(new DeslikeMapping());
  }

}