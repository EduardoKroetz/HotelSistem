using Hotel.Domain.DTOs.RoomContext.ReportDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.RoomContext;

namespace Hotel.Tests.Repositories.RoomContext;

[TestClass]
public class ReportRepositoryTest : BaseRepositoryTest
{
  private static ReportRepository ReportRepository { get; set; } = null!;

  [ClassInitialize]
  public static async Task Startup(TestContext? context)
  {
    await Startup();
    ReportRepository = new ReportRepository(MockConnection.Context);
  }
  
  [ClassCleanup]
  public static async Task Dispose()
  => await Cleanup();


  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var reports = await ReportRepository.GetByIdAsync(Reports[0].Id);

    Assert.IsNotNull(reports);
    Assert.AreEqual(Reports[0].Id, reports.Id);
    Assert.AreEqual(Reports[0].Summary, reports.Summary);
    Assert.AreEqual(Reports[0].Description, reports.Description);
    Assert.AreEqual(Reports[0].Priority, reports.Priority);
    Assert.AreEqual(Reports[0].Resolution, reports.Resolution);
    Assert.AreEqual(Reports[0].EmployeeId, reports.EmployeeId);
  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new ReportQueryParameters(0, 100, Reports[0].Summary,Reports[0].Status, null,null,null,null);
    var reports = await ReportRepository.GetAsync(parameters);

    var report = Reports.ToList()[0];

    Assert.IsNotNull(reports);
    Assert.AreEqual(Reports[0].Id, report.Id);
    Assert.AreEqual(Reports[0].Summary, report.Summary);
    Assert.AreEqual(Reports[0].Description, report.Description);
    Assert.AreEqual(Reports[0].Priority, report.Priority);
    Assert.AreEqual(Reports[0].Resolution, report.Resolution);
    Assert.AreEqual(Reports[0].EmployeeId, report.EmployeeId);
  }

  [TestMethod]
  public async Task GetAsync_WhereStatusEqualsPending_ReturnReports()
  {
    var parameters = new ReportQueryParameters(0, 100, null, EStatus.Pending, null, null, null, null);
    var reports = await ReportRepository.GetAsync(parameters);

    Assert.IsTrue(reports.Any());

    foreach (var report in reports)
      Assert.AreEqual(EStatus.Pending, report.Status);
  }

  [TestMethod]
  public async Task GetAsync_WherePriorityEqualsMedium_ReturnReports()
  {
    var parameters = new ReportQueryParameters(0, 100, null, null, EPriority.Medium, null, null, null);
    var reports = await ReportRepository.GetAsync(parameters);

    Assert.IsTrue(reports.Any());

    foreach (var report in reports)
      Assert.AreEqual(EPriority.Medium, report.Priority);
  }

  [TestMethod]
  public async Task GetAsync_WhereEmployeeId_ReturnReports()
  {
    var parameters = new ReportQueryParameters(0, 100, null, null, null, Employees[0].Id, null, null);
    var reports = await ReportRepository.GetAsync(parameters);

    Assert.IsTrue(reports.Any());

    foreach (var report in reports)
      Assert.AreEqual(Employees[0].Id, report.EmployeeId);
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtGratherThanYesterday_ReturnReports()
  {
    var parameters = new ReportQueryParameters(0, 100, null, null, null, null,DateTime.Now.AddDays(-1), "gt");
    var reports = await ReportRepository.GetAsync(parameters);

    Assert.IsTrue(reports.Any());
    foreach (var report in reports)
      Assert.IsTrue(DateTime.Now.AddDays(-1) < report.CreatedAt);

  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtLessThanToday_ReturnReports()
  {
    var parameters = new ReportQueryParameters(0, 100, null, null, null, null, DateTime.Now.AddDays(1), "lt");
    var reports = await ReportRepository.GetAsync(parameters);

    Assert.IsTrue(reports.Any());
    foreach (var report in reports)
      Assert.IsTrue(DateTime.Now.AddDays(1) > report.CreatedAt);

  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtEquals_ReturnReports()
  {
    var parameters = new ReportQueryParameters(0, 100, null, null, null, null, Reports[0].CreatedAt, "eq");
    var reports = await ReportRepository.GetAsync(parameters);

    Assert.IsTrue(reports.Any());
    foreach (var report in reports)
      Assert.AreEqual(Employees[0].CreatedAt.Date, report.CreatedAt.Date);
  }

  [TestMethod]
  public async Task GetAsync_WhereSummaryIncludesAgua_and_WherePriorityEqualsHigh_and_WhereStatusEqualsPending_ReturnReports()
  {
    var parameters = new ReportQueryParameters(0, 100, "água", EStatus.Pending, EPriority.High, null, null, null);
    var reports = await ReportRepository.GetAsync(parameters);

    Assert.IsTrue(reports.Any());
    foreach (var report in reports)
    {
      Assert.IsTrue(report.Summary.Contains("água"));
      Assert.AreEqual(EStatus.Pending, report.Status);
      Assert.AreEqual(EPriority.High, report.Priority);
    }
  }


}
