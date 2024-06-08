using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects;

namespace Hotel.Tests.UnitTests.ValueObjects;

[TestClass]
public class PhoneTest
{
    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    [DataRow("")]
    [DataRow("234")]
    [DataRow("(12) 34567-89A1")]
    [DataRow("(12) 3456-7890")]
    [DataRow("123-4567-8901")]
    [DataRow("  (12) 34567-8901  ")]
    [DataRow("+12 (34) 567-8901")]
    [DataRow("(12) 34567-8901 ext. 123")]
    [DataRow("(12) 345678901234")]
    [DataRow("(12) 345678")]
    [DataRow("1234567890")]
    [DataRow("1234-567-8901")]
    [DataRow("(12)3456-78901")]
    [DataRow("123-4567890")]
    [DataRow("12) 34567-8901")]
    [DataRow("(12) 34567-8901 ext 123")]
    public void InvalidParameters_ExpectedException(string number)
    {
        new Phone(number);
        Assert.Fail();
    }


    [TestMethod]
    [DataRow("+55 (55) 99112-1431")]
    [DataRow("+555 (555) 99112-1431")]
    public void ValidParameters_MustBeValid(string number)
    {
        var phone = new Phone(number);
        Assert.AreEqual(true, phone.IsValid);
    }
}