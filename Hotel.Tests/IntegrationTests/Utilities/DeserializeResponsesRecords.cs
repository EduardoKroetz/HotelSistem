
namespace Hotel.Tests.IntegrationTests.Utilities
{
  public record Response<T>(int Status, string Message, T Data, List<string> Errors);
  public record DataId(Guid Id);
}
