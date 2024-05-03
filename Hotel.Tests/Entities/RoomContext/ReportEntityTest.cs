using Hotel.Domain.Entities.RoomContext.ReportEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Tests.Entities.RoomContext;

[TestClass]
public class ReportEntityTest
{
  [TestMethod]
  public void ValidReport_MustBeValid()
  {
    var report = new Report("Vazamento no cano da pia","Vazamento no cano da pia do quarto 123",Domain.Enums.EPriority.High,TestParameters.Employee,"Consertar");
    Assert.IsTrue(report.IsValid);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  [DataRow("","")]
  [DataRow("Sumario",TestParameters.DescriptionMaxCaracteres)]
  [DataRow(TestParameters.DescriptionMaxCaracteres,"Descrição")]
  public void InvalidReportParameters_ExpectedException(string summary,string description)
  {
    new Report(summary,description,Domain.Enums.EPriority.High,TestParameters.Employee,"Consertar");
    Assert.Fail();
  }
}