using Hotel.Domain.Data;
using Hotel.Domain.DTOs.AdminDTOs;
using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase.Utils;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase;
using Hotel.Tests.UnitTests.Repositories.Mock;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Hotel.Domain.Entities.PermissionEntity;

namespace Hotel.Tests.UnitTests.Repositories
{
    [TestClass]
    public class AdminRepositoryTest
    {
        private readonly AdminRepository _adminRepository;
        private readonly HotelDbContext _dbContext;
        private readonly RepositoryTestUtils _utils;
        private static readonly AsyncLocal<IDbContextTransaction?> _currentTransaction = new AsyncLocal<IDbContextTransaction?>();

        public AdminRepositoryTest()
        {
            var sqliteDatabase = new SqliteDatabase();
            _dbContext = sqliteDatabase.Context;
            _adminRepository = new AdminRepository(_dbContext);
            _utils = new RepositoryTestUtils(_dbContext);
        }

        [TestInitialize]
        public async Task Initialize()
        {
            _currentTransaction.Value = await _dbContext.Database.BeginTransactionAsync();
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            if (_currentTransaction.Value != null)
            {
                await _currentTransaction.Value.RollbackAsync();
                await _currentTransaction.Value.DisposeAsync();
                _currentTransaction.Value = null;
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsWithCorrectParameters()
        {
            // Arrange
            var newAdmin = await _utils.CreateAdminAsync(new Admin(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Paulo", "Av. SP", 999)));

            // Act
            var admin = await _adminRepository.GetByIdAsync(newAdmin.Id);

            // Assert
            Assert.IsNotNull(admin);
            Assert.AreEqual(newAdmin.Id, admin.Id);
            Assert.AreEqual(newAdmin.Name.FirstName, admin.FirstName);
            Assert.AreEqual(newAdmin.Name.LastName, admin.LastName);
            Assert.AreEqual(newAdmin.Email.Address, admin.Email);
            Assert.AreEqual(newAdmin.Phone.Number, admin.Phone);
            Assert.AreEqual(newAdmin.DateOfBirth, admin.DateOfBirth);
            Assert.AreEqual(newAdmin.Address!.Country, admin.Address!.Country);
            Assert.AreEqual(newAdmin.Address.City, admin.Address.City);
            Assert.AreEqual(newAdmin.Address.Number, admin.Address.Number);
            Assert.AreEqual(newAdmin.Address.Street, admin.Address.Street);
            Assert.AreEqual(newAdmin.Gender, admin.Gender);
            Assert.AreEqual(newAdmin.CreatedAt, admin.CreatedAt);
        }

        [TestMethod]
        public async Task GetAsync_ReturnWithCorrectParameters()
        {
            // Arrange
            var newAdmin = await _utils.CreateAdminAsync(new Admin(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Paulo", "Av. SP", 999)));

            var parameters = new AdminQueryParameters(0, 1, newAdmin.Name.FirstName, null, null, null, null, null, null, null, null, null);

            // Act
            var admins = await _adminRepository.GetAsync(parameters);
            var admin = admins.ToList().FirstOrDefault();

            // Assert
            Assert.IsNotNull(admin);
            Assert.AreEqual(newAdmin.Name.FirstName, admin.FirstName);
            Assert.AreEqual(newAdmin.Name.LastName, admin.LastName);
            Assert.AreEqual(newAdmin.Email.Address, admin.Email);
            Assert.AreEqual(newAdmin.Phone.Number, admin.Phone);
            Assert.AreEqual(newAdmin.Id, admin.Id);
            Assert.AreEqual(newAdmin.IsRootAdmin, admin.IsRootAdmin);
        }

        [TestMethod]
        public async Task GetAsync_WhereName_ReturnsAdminsJoao()
        {
            // Arrange
            var newAdmin = await _utils.CreateAdminAsync(new Admin(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Paulo", "Av. SP", 999)));

            var parameters = new AdminQueryParameters(0, 1, "Lucas", null, null, null, null, null, null, null, null, null);

            // Act
            var admins = await _adminRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(admins.Any());
            foreach (var admin in admins)
            {
                Assert.AreEqual("Lucas", admin.FirstName);
            }
        }

        [TestMethod]
        public async Task GetAsync_WhereEmailFilter_ReturnsAdmins()
        {
            // Arrange
            var newAdmin = await _utils.CreateAdminAsync(new Admin(new Name("Lucas", "Silveira"), new Email("silveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Paulo", "Av. SP", 999)));

            var parameters = new AdminQueryParameters(0, 100, null, "silveira@example.com", null, null, null, null, null, null, null, null);

            // Act
            var admins = await _adminRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(admins.Any());
            foreach (var admin in admins)
            {
                Assert.AreEqual("silveira@example.com", admin.Email);
            }
        }

        [TestMethod]
        public async Task GetAsync_WherePhoneFilter_ReturnsAdmins()
        {
            // Arrange
            var newAdmin = await _utils.CreateAdminAsync(new Admin(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Paulo", "Av. SP", 999)));

            var parameters = new AdminQueryParameters(0, 100, null, null, newAdmin.Phone.Number, null, null, null, null, null, null, null);

            // Act
            var admins = await _adminRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(admins.Any());
            foreach (var admin in admins)
            {
                Assert.AreEqual(newAdmin.Phone.Number, admin.Phone);
            }
        }

        [TestMethod]
        public async Task GetAsync_WhereGenderFilter_ReturnsAdmins()
        {
            // Arrange
            var newAdmin = await _utils.CreateAdminAsync(new Admin(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Paulo", "Av. SP", 999)));

            var parameters = new AdminQueryParameters(0, 100, null, null, null, newAdmin.Gender, null, null, null, null, null, null);

            // Act
            var admins = await _adminRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(admins.Any());
            foreach (var admin in admins)
            {
                Assert.AreEqual(newAdmin.Gender, admin.Gender);
            }
        }

        [TestMethod]
        public async Task GetAsync_WhereDateOfBirthGreaterThan2000_ReturnsAdmins()
        {
            // Arrange
            var newAdmin = await _utils.CreateAdminAsync(new Admin(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Paulo", "Av. SP", 999)));

            var parameters = new AdminQueryParameters(0, 100, null, null, null, null, DateTime.Now.AddYears(-24), "gt", null, null, null, null);

            // Act
            var admins = await _adminRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(admins.Any());
            foreach (var admin in admins)
            {
                Assert.IsTrue(DateTime.Now.AddYears(-24) < admin.DateOfBirth);
            }
        }

        [TestMethod]
        public async Task GetAsync_WhereCreatedAtFilter_WhereGreaterThanOperator_ReturnsAdmins()
        {
            // Arrange
            var newAdmin = await _utils.CreateAdminAsync(new Admin(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine

, DateTime.Now.AddYears(-18), new Address("Brazil", "São Paulo", "Av. SP", 999)));

            var parameters = new AdminQueryParameters(0, 100, null, null, null, null, null, null, DateTime.Now.AddDays(-1), "gt", null, null);

            // Act
            var admins = await _adminRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(admins.Any());
            foreach (var admin in admins)
            {
                Assert.IsTrue(DateTime.Now.AddDays(-1) < admin.CreatedAt);
            }
        }

        [TestMethod]
        public async Task GetAsync_WhereCreatedAtFilter_WhereLessThanOperator_ReturnsAdmins()
        {
            // Arrange
            var newAdmin = await _utils.CreateAdminAsync(new Admin(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Paulo", "Av. SP", 999)));

            var parameters = new AdminQueryParameters(0, 100, null, null, null, null, null, null, DateTime.Now.AddDays(1), "lt", null, null);

            // Act
            var admins = await _adminRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(admins.Any());
            foreach (var admin in admins)
            {
                Assert.IsTrue(DateTime.Now.AddDays(1) > admin.CreatedAt);
            }
        }

        [TestMethod]
        public async Task GetAsync_WhereCreatedAtFilter_WhereEqualsOperator_ReturnsAdmins()
        {
            // Arrange
            var newAdmin = await _utils.CreateAdminAsync(new Admin(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Paulo", "Av. SP", 999)));

            var parameters = new AdminQueryParameters(0, 100, null, null, null, null, null, null, newAdmin.CreatedAt, "eq", null, null);

            // Act
            var admins = await _adminRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(admins.Any());
            foreach (var admin in admins)
            {
                Assert.AreEqual(newAdmin.CreatedAt, admin.CreatedAt);
            }
        }

        [TestMethod]
        public async Task GetAsync_WhereIsRootAdminEqualsFalse_ReturnsAdmins()
        {
            // Arrange
            var newAdmin = await _utils.CreateAdminAsync(new Admin(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Paulo", "Av. SP", 999)));

            var parameters = new AdminQueryParameters(0, 100, null, null, null, null, null, null, null, null, false, null);

            // Act
            var admins = await _adminRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(admins.Any());
            foreach (var admin in admins)
            {
                Assert.AreEqual(false, admin.IsRootAdmin);
            }
        }

        [TestMethod]
        public async Task GetAsync_WherePermissionId_ReturnsAdmins()
        {
            // Arrange
            var newAdmin = await _utils.CreateAdminAsync(new Admin(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Paulo", "Av. SP", 999)));
            var newPermission = await _utils.CreatePermissionAsync(new Permission("Criar admin", "Criar admin"));
            newAdmin.AddPermission(newPermission);
            await _dbContext.SaveChangesAsync();

            var parameters = new AdminQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, newPermission.Id);

            // Act
            var admins = await _adminRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(admins.Any());
            foreach (var admin in admins)
            {
                var hasPermission = await _dbContext.Admins
                  .Where(x => x.Id == admin.Id)
                  .SelectMany(x => x.Permissions)
                  .AnyAsync(x => x.Id == newPermission.Id);

                Assert.IsTrue(hasPermission);
            }
        }

        [TestMethod]
        public async Task GetAsync_WhereEmailIncludesExample_And_PhoneIncludes55_And_IsRootAdminEqualsFalse_And_GenderEqualsMasculine_ReturnsAdmins()
        {
            // Arrange
            var newAdmin = await _utils.CreateAdminAsync(new Admin(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Paulo", "Av. SP", 999)));

            var parameters = new AdminQueryParameters(0, 100, null, "example", "55", EGender.Masculine, null, null, null, null, false, null);

            // Act
            var admins = await _adminRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(admins.Any());
            foreach (var admin in admins)
            {
                Assert.IsTrue(admin.Email.Contains("example"));
                Assert.IsTrue(admin.Phone.Contains("55"));
                Assert.AreEqual(EGender.Masculine, admin.Gender);
                Assert.AreEqual(false, admin.IsRootAdmin);
            }
        }

        [TestMethod]
        public async Task GetAsync_WhereNameIncludesR_And_DateOfBirthLessThan31Years_ReturnsAdmins()
        {
            // Arrange
            var newAdmin = await _utils.CreateAdminAsync(new Admin(new Name("Rodrigo", "Faro"), new Email("Rodrigofaro@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-33), new Address("Brazil", "São Paulo", "Av. SP", 999)));

            var parameters = new AdminQueryParameters(0, 100, "R", null, null, null, DateTime.Now.AddYears(-31), "lt", null, null, null, null);

            // Act
            var admins = await _adminRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(admins.Any());
            foreach (var admin in admins)
            {
                Assert.IsTrue(admin.FirstName.Contains('R'));
                Assert.IsTrue(DateTime.Now.AddYears(-31) > admin.DateOfBirth);
                Assert.IsTrue(DateTime.Now.AddDays(1) > admin.CreatedAt);
            }
        }
    }
}

