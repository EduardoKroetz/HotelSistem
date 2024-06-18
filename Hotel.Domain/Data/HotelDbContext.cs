using Hotel.Domain.Data.Mappings;
using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.FeedbackEntity;
using Hotel.Domain.Entities.DislikeEntity;
using Hotel.Domain.Entities.EmployeeEntity;
using Hotel.Domain.Entities.ResponsibilityEntity;
using Hotel.Domain.Entities.ImageEntity;
using Hotel.Domain.Entities.LikeEntity;
using Hotel.Domain.Entities.PermissionEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.ReportEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Entities.VerificationCodeEntity;
using Microsoft.EntityFrameworkCore;
using Hotel.Domain.Entities.InvoiceEntity;

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
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<VerificationCode> VerificationCodes { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Dislike> Dislikes { get; set; }

    protected override void OnModelCreating(ModelBuilder model)
    {

        model.ApplyConfiguration(new AdminMapping());
        model.ApplyConfiguration(new PermissionMapping());
        model.ApplyConfiguration(new CustomerMapping());
        model.ApplyConfiguration(new FeedbackMapping());
        model.ApplyConfiguration(new EmployeeMapping());
        model.ApplyConfiguration(new ResponsibilityMapping());
        model.ApplyConfiguration(new InvoiceMapping());
        model.ApplyConfiguration(new ReservationMapping());
        model.ApplyConfiguration(new CategoryMapping());
        model.ApplyConfiguration(new ImageMapping());
        model.ApplyConfiguration(new ReportMapping());
        model.ApplyConfiguration(new RoomMapping());
        model.ApplyConfiguration(new ServiceMapping());
        model.ApplyConfiguration(new VerificationCodeMapping());
        model.ApplyConfiguration(new LikeMapping());
        model.ApplyConfiguration(new DislikeMapping());
    }

}