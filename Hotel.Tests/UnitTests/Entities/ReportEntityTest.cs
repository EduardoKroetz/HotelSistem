using Hotel.Domain.Entities.ReportEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;

namespace Hotel.Tests.UnitTests.Entities;

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
    public void FinishReport_WithStatusPending_MustBeFinish()
    {
        var report = new Report("Vazamento no cano da pia", "Vazamento no cano da pia do quarto 123", EPriority.High, TestParameters.Employee, "Consertar");
        report.Finish();
        Assert.AreEqual(EStatus.Finish, report.Status);
    }

    [TestMethod]
    public void FinishReport_WithStatusFinish_ShouldThrowException()
    {
        var report = new Report("Vazamento no cano da pia", "Vazamento no cano da pia do quarto 123", EPriority.High, TestParameters.Employee, "Consertar");
        report.Finish();
        var exception = Assert.ThrowsException<ValidationException>(() => report.Finish());
        Assert.AreEqual("O relat�rio j� est� finalizado", exception.Message);
    }

    [TestMethod]
    public void FinishReport_WithStatusCancelled_ShouldThrowException()
    {
        var report = new Report("Vazamento no cano da pia", "Vazamento no cano da pia do quarto 123", EPriority.High, TestParameters.Employee, "Consertar");
        report.Cancel();
        var exception = Assert.ThrowsException<ValidationException>(() => report.Finish());
        Assert.AreEqual("N�o � poss�vel finalizar um relat�rio cancelado", exception.Message);
    }

    [TestMethod]
    public void CancelReport_WithStatusPending_MustBeCancel()
    {
        var report = new Report("Vazamento no cano da pia", "Vazamento no cano da pia do quarto 123", EPriority.High, TestParameters.Employee, "Consertar");
        report.Cancel();
        Assert.AreEqual(EStatus.Cancelled, report.Status);
    }

    [TestMethod]
    public void CancelReport_WithStatusFinish_ShouldThrowException()
    {
        var report = new Report("Vazamento no cano da pia", "Vazamento no cano da pia do quarto 123", EPriority.High, TestParameters.Employee, "Consertar");
        report.Finish();
        var exception = Assert.ThrowsException<ValidationException>(() => report.Cancel());
        Assert.AreEqual("N�o � poss�vel cancelar um relat�rio finalizado", exception.Message);
    }

    [TestMethod]
    public void CancelReport_WithStatusCancelled_ShouldThrowException()
    {
        var report = new Report("Vazamento no cano da pia", "Vazamento no cano da pia do quarto 123", EPriority.High, TestParameters.Employee, "Consertar");
        report.Cancel();
        var exception = Assert.ThrowsException<ValidationException>(() => report.Cancel());
        Assert.AreEqual("O relat�rio j� est� cancelado", exception.Message);
    }
}
