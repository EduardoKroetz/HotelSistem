using Hotel.Domain.Data;
using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.DislikeEntity;
using Hotel.Domain.Entities.EmployeeEntity;
using Hotel.Domain.Entities.FeedbackEntity;
using Hotel.Domain.Entities.LikeEntity;
using Hotel.Domain.Entities.PermissionEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.RoomEntity;

namespace Hotel.Tests.UnitTests.Repositories.InMemoryDatabase.Utils;

public class RepositoryTestUtils
{
    private readonly HotelDbContext _dbContext;

    public RepositoryTestUtils(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Admin> CreateAdminAsync(Admin newAdmin)
    {
        await _dbContext.Admins.AddAsync(newAdmin);
        await _dbContext.SaveChangesAsync();
        return newAdmin;
    }

    public async Task<Permission> CreatePermissionAsync(Permission newPermission)
    {
        await _dbContext.Permissions.AddAsync(newPermission);
        await _dbContext.SaveChangesAsync();
        return newPermission;
    }

    public async Task<Category> CreateCategoryAsync(Category newCategory)
    {
        await _dbContext.Categories.AddAsync(newCategory);
        await _dbContext.SaveChangesAsync();
        return newCategory;
    }

    public async Task<Room> CreateRoomAsync(Room newRoom)
    {
        await _dbContext.Rooms.AddAsync(newRoom);
        await _dbContext.SaveChangesAsync();
        return newRoom;
    }

    public async Task<Customer> CreateCustomerAsync(Customer newCustomer)
    {
        await _dbContext.Customers.AddAsync(newCustomer);
        await _dbContext.SaveChangesAsync();
        return newCustomer;
    }

    public async Task<Employee> CreateEmployeeAsync(Employee newEmployee)
    {
        await _dbContext.Employees.AddAsync(newEmployee);
        await _dbContext.SaveChangesAsync();
        return newEmployee;
    }

    public async Task<Feedback> CreateFeedbackAsync(Feedback newFeedback)
    {
        await _dbContext.Feedbacks.AddAsync(newFeedback);
        await _dbContext.SaveChangesAsync();
        return newFeedback;
    }

    public async Task<Reservation> CreateReservationAsync(Reservation newReservation)
    {
        await _dbContext.Reservations.AddAsync(newReservation);
        await _dbContext.SaveChangesAsync();
        return newReservation;
    }

    public async Task<Dislike> CreateDislikeAsync(Dislike newDislike)
    {
        await _dbContext.Dislikes.AddAsync(newDislike);
        await _dbContext.SaveChangesAsync();
        return newDislike;
    }

    public async Task<Like> CreateLikeAsync(Like newLike)
    {
        await _dbContext.Likes.AddAsync(newLike);
        await _dbContext.SaveChangesAsync();
        return newLike;
    }

}
