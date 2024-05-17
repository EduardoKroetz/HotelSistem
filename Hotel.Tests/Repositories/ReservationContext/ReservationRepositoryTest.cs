using Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;
using Hotel.Domain.Repositories.ReservationContext;
using Hotel.Tests.Repositories.Mock;


namespace Hotel.Tests.Repositories.ReservationContext;

[TestClass]
public class ReservationRepositoryTest
{
  private static ReservationRepository ReservationRepository { get; set; }

  static ReservationRepositoryTest()
  => ReservationRepository = new ReservationRepository(BaseRepositoryTest.MockConnection.Context);

  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var reservation = await ReservationRepository.GetByIdAsync(BaseRepositoryTest.Reservations[0].Id);

    Assert.IsNotNull(reservation);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].Id, reservation.Id);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].DailyRate, reservation.DailyRate);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].HostedDays, reservation.HostedDays);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].CheckIn, reservation.CheckIn);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].CheckOut, reservation.CheckOut);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].Status, reservation.Status);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].Capacity, reservation.Capacity);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].RoomId, reservation.RoomId);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].InvoiceId, reservation.InvoiceId);
  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new ReservationQueryParameters(0, 100, BaseRepositoryTest.Reservations[0].HostedDays,"eq", BaseRepositoryTest.Reservations[0].DailyRate, "eq", BaseRepositoryTest.Reservations[0].CheckIn, "eq", BaseRepositoryTest.Reservations[0].CheckOut, "eq", BaseRepositoryTest.Reservations[0].Status, BaseRepositoryTest.Reservations[0].Capacity,"eq", BaseRepositoryTest.Reservations[0].RoomId, null, BaseRepositoryTest.Reservations[0].InvoiceId, null, null, null);
    var reservations = await ReservationRepository.GetAsync(parameters);

    var reservation = reservations.ToList()[0];

    Assert.IsNotNull(reservation);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].Id, reservation.Id);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].DailyRate, reservation.DailyRate);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].HostedDays, reservation.HostedDays);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].CheckIn, reservation.CheckIn);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].CheckOut, reservation.CheckOut);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].Status, reservation.Status);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].Capacity, reservation.Capacity);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].RoomId, reservation.RoomId);
    Assert.AreEqual(BaseRepositoryTest.Reservations[0].InvoiceId, reservation.InvoiceId);
  }

  [TestMethod]
  public async Task GetAsync_WhereHostedDaysGratherThan2_ReturnsReservations()
  {
    var parameters = new ReservationQueryParameters(0, 100, 2, "gt", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
    var reservations = await ReservationRepository.GetAsync(parameters);

    Assert.IsTrue(reservations.Any());

    foreach (var reservation in reservations)
      Assert.IsTrue(2 < reservation.HostedDays);
    
  }

  [TestMethod]
  public async Task GetAsync_WhereHostedDaysLessThan1_ReturnsReservations()
  {
    var parameters = new ReservationQueryParameters(0, 100, 1, "lt", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
    var reservations = await ReservationRepository.GetAsync(parameters);

    Assert.IsTrue(reservations.Any());

    foreach (var reservation in reservations)
      Assert.IsTrue(1 > reservation.HostedDays);
   
  }

  [TestMethod]
  public async Task GetAsync_WhereHostedDaysEquals2_ReturnsReservations()
  {
    var parameters = new ReservationQueryParameters(0, 100, 0, "eq", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
    var reservations = await ReservationRepository.GetAsync(parameters);

    Assert.IsTrue(reservations.Any());

    foreach (var reservation in reservations)
      Assert.AreEqual(0,reservation.HostedDays);
  }

  [TestMethod]
  public async Task GetAsync_WhereDailyRateGratherThan70_ReturnsReservations()
  {
    var parameters = new ReservationQueryParameters(0, 100, null, null, 70, "gt", null, null, null, null, null, null, null, null, null, null, null, null, null);
    var reservations = await ReservationRepository.GetAsync(parameters);

    Assert.IsTrue(reservations.Any());

    foreach (var reservation in reservations)
      Assert.IsTrue(70 < reservation.DailyRate);

  }

  [TestMethod]
  public async Task GetAsync_WhereDailyRateLessThan120_ReturnsReservations()
  {
    var parameters = new ReservationQueryParameters(0, 100, null, null, 120, "lt", null, null, null, null, null, null, null, null, null, null, null, null, null);
    var reservations = await ReservationRepository.GetAsync(parameters);

    Assert.IsTrue(reservations.Any());

    foreach (var reservation in reservations)
      Assert.IsTrue(120 > reservation.DailyRate);

  }

  [TestMethod]
  public async Task GetAsync_WhereDailyRateEquals50_ReturnsReservations()
  {
    var parameters = new ReservationQueryParameters(0, 100, null, null, 50, "eq", null, null, null, null, null, null, null, null, null, null, null, null, null);
    var reservations = await ReservationRepository.GetAsync(parameters);

    Assert.IsTrue(reservations.Any());

    foreach (var reservation in reservations)
      Assert.AreEqual(50, reservation.DailyRate);
  }

  [TestMethod]
  public async Task GetAsync_WhereCheckInGratherThanYesterday_ReturnsReservations()
  {
    var parameters = new ReservationQueryParameters(0, 100, null, null,null,null, DateTime.Now.AddDays(-1), "gt", null, null, null, null, null, null, null, null, null, null, null);
    var reservations = await ReservationRepository.GetAsync(parameters);

    Assert.IsTrue(reservations.Any());

    foreach (var reservation in reservations)
      Assert.IsTrue(DateTime.Now.AddDays(-1) < reservation.CheckIn);

  }

  [TestMethod]
  public async Task GetAsync_WhereCheckInLessThanTomorrow_ReturnsReservations()
  {
    var parameters = new ReservationQueryParameters(0, 100, null, null, null, null, DateTime.Now.AddDays(1), "lt", null, null, null, null, null, null, null, null, null, null, null);
    var reservations = await ReservationRepository.GetAsync(parameters);

    Assert.IsTrue(reservations.Any());

    foreach (var reservation in reservations)
      Assert.IsTrue(DateTime.Now.AddDays(1) > reservation.CheckIn);

  }

  [TestMethod]
  public async Task GetAsync_WhereCheckInEqualsNow_ReturnsReservations()
  {
    var parameters = new ReservationQueryParameters(0, 100, null, null, null, null, BaseRepositoryTest.Reservations[0].CheckIn, "eq", null, null, null, null, null, null, null, null, null, null, null);
    var reservations = await ReservationRepository.GetAsync(parameters);

    Assert.IsTrue(reservations.Any());

    foreach (var reservation in reservations)
      Assert.AreEqual(BaseRepositoryTest.Reservations[0].CheckIn, reservation.CheckIn);
  }

  [TestMethod]
  public async Task GetAsync_WhereCheckOutGratherThanAfter2Days_ReturnsReservations()
  {
    var parameters = new ReservationQueryParameters(0, 100, null, null, null, null, null, null, DateTime.Now.AddDays(2), "gt", null, null, null, null, null, null, null, null, null);
    var reservations = await ReservationRepository.GetAsync(parameters);

    Assert.IsTrue(reservations.Any());

    foreach (var reservation in reservations)
      Assert.IsTrue(DateTime.Now.AddDays(2) < reservation.CheckOut);

  }

  [TestMethod]
  public async Task GetAsync_WhereCheckOutLessThanAfter5Days_ReturnsReservations()
  {
    var parameters = new ReservationQueryParameters(0, 100, null, null, null, null,  null, null, DateTime.Now.AddDays(5), "lt", null, null, null, null, null, null, null, null, null);
    var reservations = await ReservationRepository.GetAsync(parameters);

    Assert.IsTrue(reservations.Any());

    foreach (var reservation in reservations)
      Assert.IsTrue(DateTime.Now.AddDays(5) > reservation.CheckOut);

  }

  [TestMethod]
  public async Task GetAsync_WhereCheckOutEquals_ReturnsReservations()
  {
    var parameters = new ReservationQueryParameters(0, 100, null, null, null, null,  null, null, BaseRepositoryTest.Reservations[0].CheckOut, "eq", null, null, null, null, null, null, null, null, null);
    var reservations = await ReservationRepository.GetAsync(parameters);

    Assert.IsTrue(reservations.Any());

    foreach (var reservation in reservations)
      Assert.AreEqual(BaseRepositoryTest.Reservations[0].CheckOut, reservation.CheckOut);
  }
 

}
