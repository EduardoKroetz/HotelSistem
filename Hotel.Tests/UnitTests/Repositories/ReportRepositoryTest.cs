using Hotel.Domain.Data;
using Hotel.Domain.DTOs.ReportDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase.Utils;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase;
using Microsoft.EntityFrameworkCore.Storage;
using Hotel.Domain.Entities.ReportEntity;
using Hotel.Domain.Entities.EmployeeEntity;
using Hotel.Domain.ValueObjects;

namespace Hotel.Tests.UnitTests.Repositories;

[TestClass]
public class ReportRepositoryTest
{
    private readonly ReportRepository _reportRepository;
    private readonly HotelDbContext _dbContext;
    private readonly RepositoryTestUtils _utils;
    private static readonly AsyncLocal<IDbContextTransaction?> _currentTransaction = new AsyncLocal<IDbContextTransaction?>();

    public ReportRepositoryTest()
    {
        var sqliteDatabase = new SqliteDatabase();
        _dbContext = sqliteDatabase.Context;
        _reportRepository = new ReportRepository(_dbContext);
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

    private async Task<Report> CreateReportAsync()
    {
        var newEmployee = await _utils.CreateEmployeeAsync(new Employee(
            new Name("Ana", "Santos"),
            new Email("anasantos@example.com"),
            new Phone("+55 (31) 98765-9121"),
            "ana456",
            EGender.Feminine,
            DateTime.Now.AddYears(-28),
            new Address("Brazil", "Belo Horizonte", "Avenida Afonso Pena", 456),
            400));

        return await _utils.CreateReportAsync(new Report(
            "Erro no quarto 30",
            "Erro no quarto 30",
            EPriority.High,
            newEmployee));
    }

    [TestMethod]
    public async Task GetByIdAsync_ReturnsWithCorrectParameters()
    {
        // Arrange
        var newReport = await CreateReportAsync();

        // Act
        var reports = await _reportRepository.GetByIdAsync(newReport.Id);

        // Assert
        Assert.IsNotNull(reports);
        Assert.AreEqual(newReport.Id, reports.Id);
        Assert.AreEqual(newReport.Summary, reports.Summary);
        Assert.AreEqual(newReport.Description, reports.Description);
        Assert.AreEqual(newReport.Priority, reports.Priority);
        Assert.AreEqual(newReport.Resolution, reports.Resolution);
        Assert.AreEqual(newReport.EmployeeId, reports.EmployeeId);
    }

    [TestMethod]
    public async Task GetAsync_ReturnWithCorrectParameters()
    {
        // Arrange
        var newReport = await CreateReportAsync();
        var parameters = new ReportQueryParameters(0, 100, newReport.Summary, newReport.Status, null, null, null, null);

        // Act
        var reports = await _reportRepository.GetAsync(parameters);
        var report = reports.FirstOrDefault();

        // Assert
        Assert.IsNotNull(report);
        Assert.AreEqual(newReport.Id, report.Id);
        Assert.AreEqual(newReport.Summary, report.Summary);
        Assert.AreEqual(newReport.Description, report.Description);
        Assert.AreEqual(newReport.Priority, report.Priority);
        Assert.AreEqual(newReport.Resolution, report.Resolution);
        Assert.AreEqual(newReport.EmployeeId, report.EmployeeId);
    }

    [TestMethod]
    public async Task GetAsync_WhereStatusEqualsPending_ReturnReports()
    {
        // Arrange
        var newReport = await CreateReportAsync();
        var parameters = new ReportQueryParameters(0, 100, null, EStatus.Pending, null, null, null, null);

        // Act
        var reports = await _reportRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(reports.Any());
        foreach (var report in reports)
        {
            Assert.AreEqual(EStatus.Pending, report.Status);
        }
    }

    [TestMethod]
    public async Task GetAsync_WherePriorityEqualsMedium_ReturnReports()
    {
        // Arrange
        var newEmployee = await _utils.CreateEmployeeAsync(new Employee(new Name("Ana", "Santos"), new Email("anasantos@example.com"), new Phone("+55 (31) 98765-9121"), "ana456", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Belo Horizonte", "Avenida Afonso Pena", 456), 400));
        var newReport = await _utils.CreateReportAsync(new Report("Erro no quarto 30", "Erro no quarto 30", EPriority.Medium, newEmployee));
        var parameters = new ReportQueryParameters(0, 100, null, null, EPriority.Medium, null, null, null);

        // Act
        var reports = await _reportRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(reports.Any());
        foreach (var report in reports)
        {
            Assert.AreEqual(EPriority.Medium, report.Priority);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereEmployeeId_ReturnReports()
    {
        // Arrange
        var newReport = await CreateReportAsync();
        var parameters = new ReportQueryParameters(0, 100, null, null, null, newReport.EmployeeId, null, null);

        // Act
        var reports = await _reportRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(reports.Any());
        foreach (var report in reports)
        {
            Assert.AreEqual(newReport.EmployeeId, report.EmployeeId);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereSummaryIncludesAgua_and_WherePriorityEqualsHigh_and_WhereStatusEqualsPending_ReturnReports()
    {
        // Arrange
        var newEmployee = await _utils.CreateEmployeeAsync(new Employee(new Name("Ana", "Santos"), new Email("anasantos@example.com"), new Phone("+55 (31) 98765-9121"), "ana456", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Belo Horizonte", "Avenida Afonso Pena", 456), 400));
        var newReport = await _utils.CreateReportAsync(new Report("Vazamento de água", "Vazamento de água no cômodo 1", EPriority.High, newEmployee));

        var parameters = new ReportQueryParameters(0, 100, "água", EStatus.Pending, EPriority.High, null, null, null);

        // Act
        var reports = await _reportRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(reports.Any());
        foreach (var report in reports)
        {
            Assert.IsTrue(report.Summary.Contains("água"));
            Assert.AreEqual(EStatus.Pending, report.Status);
            Assert.AreEqual(EPriority.High, report.Priority);
        }
    }
}
