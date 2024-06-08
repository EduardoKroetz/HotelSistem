using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.UnitTests.Entities;

namespace Hotel.Tests.UnitTests.ValueObjects;

[TestClass]
public class EmailTest
{
    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    [DataRow("")]
    [DataRow("a@a.a")]
    [DataRow("aa.a")]
    [DataRow("aaa")]
    [DataRow(TestParameters.DescriptionMaxCaracteres)]
    [DataRow(".com")]
    [DataRow("@gmail.com")]
    [DataRow("alert@.com")]
    [DataRow("batman@gmail.")]
    [DataRow("batman@")]
    public void InvalidParameters_ExpectedException(string address)
    {
        new Email(address);
        Assert.Fail();
    }

    [TestMethod]
    [DataRow("batman@gmail.com")]
    [DataRow("bat@outlook.com")]
    public void ValidParameters_MustBeValid(string address)
    {
        var email = new Email(address);
        Assert.AreEqual(true, email.IsValid);
    }
}