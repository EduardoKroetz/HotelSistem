using Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;
using Hotel.Domain.Repositories.ReservationContext;
using Hotel.Tests.UnitTests.Repositories.Mock;
using Microsoft.EntityFrameworkCore;


namespace Hotel.Tests.UnitTests.Repositories.ReservationContext;

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
        Assert.AreEqual(BaseRepositoryTest.Reservations[0].TimeHosted, reservation.TimeHosted);
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
        var parameters = new ReservationQueryParameters(0, 100, BaseRepositoryTest.Reservations[0].TimeHosted, "eq", BaseRepositoryTest.Reservations[0].DailyRate, "eq", BaseRepositoryTest.Reservations[0].CheckIn, "eq", BaseRepositoryTest.Reservations[0].CheckOut, "eq", BaseRepositoryTest.Reservations[0].Status, BaseRepositoryTest.Reservations[0].Capacity, "eq", BaseRepositoryTest.Reservations[0].RoomId, null, BaseRepositoryTest.Reservations[0].InvoiceId, null, null, null, null, null, null, null, null, null);
        var reservations = await ReservationRepository.GetAsync(parameters);

        var reservation = reservations.ToList()[0];

        Assert.IsNotNull(reservation);
        Assert.AreEqual(BaseRepositoryTest.Reservations[0].Id, reservation.Id);
        Assert.AreEqual(BaseRepositoryTest.Reservations[0].DailyRate, reservation.DailyRate);
        Assert.AreEqual(BaseRepositoryTest.Reservations[0].TimeHosted, reservation.TimeHosted);
        Assert.AreEqual(BaseRepositoryTest.Reservations[0].CheckIn, reservation.CheckIn);
        Assert.AreEqual(BaseRepositoryTest.Reservations[0].CheckOut, reservation.CheckOut);
        Assert.AreEqual(BaseRepositoryTest.Reservations[0].Status, reservation.Status);
        Assert.AreEqual(BaseRepositoryTest.Reservations[0].Capacity, reservation.Capacity);
        Assert.AreEqual(BaseRepositoryTest.Reservations[0].RoomId, reservation.RoomId);
        Assert.AreEqual(BaseRepositoryTest.Reservations[0].InvoiceId, reservation.InvoiceId);
    }

    [TestMethod]
    public async Task GetAsync_WhereTimeHostedLessThan24Hours_ReturnsReservations()
    {
        var parameters = new ReservationQueryParameters(0, 100, TimeSpan.FromDays(1), "lt", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
        var reservations = await ReservationRepository.GetAsync(parameters);

        Assert.IsTrue(reservations.Any());

        foreach (var reservation in reservations)
            Assert.IsTrue(TimeSpan.FromDays(1) > reservation.TimeHosted);

    }

    [TestMethod]
    public async Task GetAsync_WhereTimeHostedEquals_ReturnsReservations()
    {
        var reservationWithTimeHosted = await BaseRepositoryTest.MockConnection.Context.Reservations.Where(x => x.TimeHosted != null).FirstOrDefaultAsync();

        var parameters = new ReservationQueryParameters(0, 100, reservationWithTimeHosted!.TimeHosted, "eq", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
        var reservations = await ReservationRepository.GetAsync(parameters);

        Assert.IsTrue(reservations.Any());

        foreach (var reservation in reservations)
            Assert.AreEqual(reservationWithTimeHosted.TimeHosted, reservation.TimeHosted);
    }

    [TestMethod]
    public async Task GetAsync_WhereDailyRateGratherThan70_ReturnsReservations()
    {
        var parameters = new ReservationQueryParameters(0, 100, null, null, 70, "gt", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
        var reservations = await ReservationRepository.GetAsync(parameters);

        Assert.IsTrue(reservations.Any());

        foreach (var reservation in reservations)
            Assert.IsTrue(70 < reservation.DailyRate);

    }

    [TestMethod]
    public async Task GetAsync_WhereDailyRateLessThan120_ReturnsReservations()
    {
        var parameters = new ReservationQueryParameters(0, 100, null, null, 120, "lt", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
        var reservations = await ReservationRepository.GetAsync(parameters);

        Assert.IsTrue(reservations.Any());

        foreach (var reservation in reservations)
            Assert.IsTrue(120 > reservation.DailyRate);

    }

    [TestMethod]
    public async Task GetAsync_WhereDailyRateEquals_ReturnsReservations()
    {
        var parameters = new ReservationQueryParameters(0, 100, null, null, BaseRepositoryTest.Reservations[0].DailyRate, "eq", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
        var reservations = await ReservationRepository.GetAsync(parameters);

        Assert.IsTrue(reservations.Any());

        foreach (var reservation in reservations)
            Assert.AreEqual(BaseRepositoryTest.Reservations[0].DailyRate, reservation.DailyRate);
    }

    [TestMethod]
    public async Task GetAsync_WhereCheckInGratherThanLeastOneDay_ReturnsReservations()
    {
        var parameters = new ReservationQueryParameters(0, 100, null, null, null, null, DateTime.Now.AddDays(-1), "gt", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
        var reservations = await ReservationRepository.GetAsync(parameters);

        Assert.IsTrue(reservations.Any());

        foreach (var reservation in reservations)
            Assert.IsTrue(DateTime.Now.AddDays(-1) < reservation.CheckIn);

    }

    [TestMethod]
    public async Task GetAsync_WhereCheckInLessThanAfter1Days_ReturnsReservations()
    {
        var parameters = new ReservationQueryParameters(0, 100, null, null, null, null, DateTime.Now.AddDays(1), "lt", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
        var reservations = await ReservationRepository.GetAsync(parameters);

        Assert.IsTrue(reservations.Any());

        foreach (var reservation in reservations)
            Assert.IsTrue(DateTime.Now.AddDays(1) > reservation.CheckIn);

    }

    [TestMethod]
    public async Task GetAsync_WhereCheckInEquals_ReturnsReservations()
    {
        var reservationWithCheckIn = await BaseRepositoryTest.MockConnection.Context.Reservations.Where(x => x.CheckIn != null).FirstOrDefaultAsync();

        var parameters = new ReservationQueryParameters(0, 100, null, null, null, null, reservationWithCheckIn!.CheckIn, "eq", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
        var reservations = await ReservationRepository.GetAsync(parameters);

        Assert.IsTrue(reservations.Any());

        foreach (var reservation in reservations)
            Assert.AreEqual(reservationWithCheckIn!.CheckIn, reservation.CheckIn);
    }

    [TestMethod]
    public async Task GetAsync_WhereCheckOutGratherThanYesterday_ReturnsReservations()
    {
        var parameters = new ReservationQueryParameters(0, 100, null, null, null, null, null, null, DateTime.Now.AddDays(-1), "gt", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
        var reservations = await ReservationRepository.GetAsync(parameters);

        Assert.IsTrue(reservations.Any());

        foreach (var reservation in reservations)
            Assert.IsTrue(DateTime.Now.AddDays(-1) < reservation.CheckOut);

    }

    [TestMethod]
    public async Task GetAsync_WhereCheckOutLessThanAfter1Days_ReturnsReservations()
    {
        var parameters = new ReservationQueryParameters(0, 100, null, null, null, null, null, null, DateTime.Now.AddDays(1), "lt", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
        var reservations = await ReservationRepository.GetAsync(parameters);

        Assert.IsTrue(reservations.Any());

        foreach (var reservation in reservations)
            Assert.IsTrue(DateTime.Now.AddDays(1) > reservation.CheckOut);

    }

    [TestMethod]
    public async Task GetAsync_WhereCheckOutEquals_ReturnsReservations()
    {
        var reservationWithCheckOut = await BaseRepositoryTest.MockConnection.Context.Reservations.Where(x => x.CheckOut != null).FirstOrDefaultAsync();

        var parameters = new ReservationQueryParameters(0, 100, null, null, null, null, null, null, reservationWithCheckOut!.CheckOut, "eq", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
        var reservations = await ReservationRepository.GetAsync(parameters);

        Assert.IsTrue(reservations.Any());

        foreach (var reservation in reservations)
            Assert.AreEqual(reservationWithCheckOut.CheckOut, reservation.CheckOut);
    }


}
