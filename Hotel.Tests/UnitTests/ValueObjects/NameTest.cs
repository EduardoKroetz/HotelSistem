using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects;

namespace Hotel.Tests.UnitTests.ValueObjects;

[TestClass]
public class NameTest
{
    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    [DataRow("", "")]
    public void InvalidParameters_ExpectedException(string firstName, string lastName)
    {
        new Name(firstName, lastName);
        Assert.Fail();
    }

}