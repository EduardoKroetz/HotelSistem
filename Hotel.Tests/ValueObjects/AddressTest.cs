using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects;

namespace Hotel.Tests.ValueObjects;

[TestClass]
public class AddressTest
{
  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  [DataRow("","Gotham","Batman street",163)]
  [DataRow("Estados Unidos da América","","Batman street",163)]
  [DataRow("Estados Unidos da América","Gotham","",163)]
  [DataRow("Estados Unidos da América","Gotham","Batman street",0)]
  [DataRow("Estados Unidos da América","Gotham","Batman street",-1)]
  public void InvalidParameters_ExpectedException(string country,string city,string street, int number)
  {
    new Address(country,city,street,number);
    Assert.Fail();
  }
}