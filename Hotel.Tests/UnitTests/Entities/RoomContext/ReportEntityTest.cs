using Hotel.Domain.Entities.RoomContext.ReportEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;
using Hotel.Tests.UnitTests.Entities;

namespace Hotel.Tests.UnitTests.Entities.RoomContext;

[TestClass]
public class ReportEntityTest
{
    [TestMethod]
    public void ValidReport_MustBeValid()
    {
        var report = new Report("Vazamento no cano da pia", "Vazamento no cano da pia do quarto 123", EPriority.High, TestParameters.Employee, "Consertar");
        Assert.IsTrue(report.IsValid);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    [DataRow("", "")]
    [DataRow(TestParameters.DescriptionMaxCaracteres, "Descricao")]
    public void InvalidReportParameters_ExpectedException(string summary, string description)
    {
        new Report(summary, description, EPriority.High, TestParameters.Employee, "Consertar");
        Assert.Fail();
    }

    [TestMethod]
    public void FinishReport_WithStatusPending_MustBeFinish()
    {
        var report = new Report("Vazamento no cano da pia", "Vazamento no cano da pia do quarto 123", EPriority.High, TestParameters.Employee, "Consertar");
        report.Finish();
        Assert.AreEqual(EStatus.Finish, report.Status);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void FinishReport_WithStatusFinish_ExpectedException()
    {
        var report = new Report("Vazamento no cano da pia", "Vazamento no cano da pia do quarto 123", EPriority.High, TestParameters.Employee, "Consertar");
        report.Finish();
        report.Finish();
        Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void FinishReport_WithStatusCancelled_ExpectedException()
    {
        var report = new Report("Vazamento no cano da pia", "Vazamento no cano da pia do quarto 123", EPriority.High, TestParameters.Employee, "Consertar");
        report.Cancel();
        report.Finish();
        Assert.Fail();
    }

    [TestMethod]
    public void CancelReport_WithStatusPending_MustBeCancel()
    {
        var report = new Report("Vazamento no cano da pia", "Vazamento no cano da pia do quarto 123", EPriority.High, TestParameters.Employee, "Consertar");
        report.Cancel();
        Assert.AreEqual(EStatus.Cancelled, report.Status);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void CancelReport_WithStatusFinish_ExpectedException()
    {
        var report = new Report("Vazamento no cano da pia", "Vazamento no cano da pia do quarto 123", EPriority.High, TestParameters.Employee, "Consertar");
        report.Finish();
        report.Cancel();
        Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void CancelReport_WithStatusCancelled_ExpectedException()
    {
        var report = new Report("Vazamento no cano da pia", "Vazamento no cano da pia do quarto 123", EPriority.High, TestParameters.Employee, "Consertar");
        report.Cancel();
        report.Cancel();
        Assert.Fail();
    }
}