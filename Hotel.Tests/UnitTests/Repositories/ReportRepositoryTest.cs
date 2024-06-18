using Hotel.Domain.DTOs.ReportDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories;
using Hotel.Tests.UnitTests.Repositories.Mock;

namespace Hotel.Tests.UnitTests.Repositories;

[TestClass]
public class ReportRepositoryTest
{
    private static ReportRepository ReportRepository { get; set; }

    static ReportRepositoryTest()
    => ReportRepository = new ReportRepository(BaseRepositoryTest.MockConnection.Context);


    [TestMethod]
    public async Task GetByIdAsync_ReturnsWithCorrectParameters()
    {
        var reports = await ReportRepository.GetByIdAsync(BaseRepositoryTest.Reports[0].Id);

        Assert.IsNotNull(reports);
        Assert.AreEqual(BaseRepositoryTest.Reports[0].Id, reports.Id);
        Assert.AreEqual(BaseRepositoryTest.Reports[0].Summary, reports.Summary);
        Assert.AreEqual(BaseRepositoryTest.Reports[0].Description, reports.Description);
        Assert.AreEqual(BaseRepositoryTest.Reports[0].Priority, reports.Priority);
        Assert.AreEqual(BaseRepositoryTest.Reports[0].Resolution, reports.Resolution);
        Assert.AreEqual(BaseRepositoryTest.Reports[0].EmployeeId, reports.EmployeeId);
    }

    [TestMethod]
    public async Task GetAsync_ReturnWithCorrectParameters()
    {
        var parameters = new ReportQueryParameters(0, 100, BaseRepositoryTest.Reports[0].Summary, BaseRepositoryTest.Reports[0].Status, null, null, null, null);
        var reports = await ReportRepository.GetAsync(parameters);

        var report = BaseRepositoryTest.Reports.ToList()[0];

        Assert.IsNotNull(reports);
        Assert.AreEqual(BaseRepositoryTest.Reports[0].Id, report.Id);
        Assert.AreEqual(BaseRepositoryTest.Reports[0].Summary, report.Summary);
        Assert.AreEqual(BaseRepositoryTest.Reports[0].Description, report.Description);
        Assert.AreEqual(BaseRepositoryTest.Reports[0].Priority, report.Priority);
        Assert.AreEqual(BaseRepositoryTest.Reports[0].Resolution, report.Resolution);
        Assert.AreEqual(BaseRepositoryTest.Reports[0].EmployeeId, report.EmployeeId);
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
        var parameters = new ReportQueryParameters(0, 100, null, null, null, BaseRepositoryTest.Employees[0].Id, null, null);
        var reports = await ReportRepository.GetAsync(parameters);

        Assert.IsTrue(reports.Any());

        foreach (var report in reports)
            Assert.AreEqual(BaseRepositoryTest.Employees[0].Id, report.EmployeeId);
    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtGratherThanYesterday_ReturnReports()
    {
        var parameters = new ReportQueryParameters(0, 100, null, null, null, null, DateTime.Now.AddDays(-1), "gt");
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
        var parameters = new ReportQueryParameters(0, 100, null, null, null, null, BaseRepositoryTest.Reports[0].CreatedAt, "eq");
        var reports = await ReportRepository.GetAsync(parameters);

        Assert.IsTrue(reports.Any());
        foreach (var report in reports)
            Assert.AreEqual(BaseRepositoryTest.Employees[0].CreatedAt.Date, report.CreatedAt.Date);
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
