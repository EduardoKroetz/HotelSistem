using Hotel.Domain.Data;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Extensions;
using Hotel.Domain.Repositories.Interfaces.ReservationContext;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories.ReservationContext;

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
  public ReservationRepository(HotelDbContext context) : base(context) { }
  public async Task<GetReservation?> GetByIdAsync(Guid id)
  {
    return await _context
      .Reservations
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Include(x => x.Customer)
      .Include(x => x.Services)
      .Select(x => new GetReservation(
        x.Id,
        x.DailyRate,
        x.ExpectedTimeHosted,
        x.ExpectedCheckIn,
        x.ExpectedCheckOut,
        x.TimeHosted,
        x.CheckIn,
        x.CheckOut,
        x.Status,
        x.Capacity,
        x.RoomId,
        x.CustomerId,
        x.InvoiceId))
      .FirstOrDefaultAsync();

  }

  public async Task<IEnumerable<GetReservation>> GetAsync(ReservationQueryParameters queryParameters)
  {
    var query = _context.Reservations.AsQueryable();

    if (queryParameters.TimeHosted.HasValue)
      query = query.FilterByOperator(queryParameters.TimeHostedOperator,x => x.TimeHosted,queryParameters.TimeHosted);

    if (queryParameters.DailyRate.HasValue)
      query = query.FilterByOperator(queryParameters.DailyRateOperator, x => x.DailyRate, queryParameters.DailyRate);

    if (queryParameters.CheckIn.HasValue)
      query = query.FilterByOperator(queryParameters.CheckInOperator, x => x.CheckIn, queryParameters.CheckIn);

    if (queryParameters.CheckOut.HasValue)
      query = query.FilterByOperator(queryParameters.CheckOutOperator, x => x.CheckOut, queryParameters.CheckOut);

    if (queryParameters.Status.HasValue)
      query = query.Where(x => x.Status == queryParameters.Status);

    if (queryParameters.Capacity.HasValue)
      query = query.FilterByOperator(queryParameters.CapacityOperator, x => x.Capacity, queryParameters.Capacity);

    if (queryParameters.CustomerId.HasValue)
      query = query.Where(x => x.CustomerId == queryParameters.CustomerId);

    if (queryParameters.RoomId.HasValue)
      query = query.Where(x => x.RoomId == queryParameters.RoomId);

    if (queryParameters.InvoiceId.HasValue)
      query = query.Where(x => x.InvoiceId == queryParameters.InvoiceId);

    if (queryParameters.ServiceId.HasValue)
      query = query.Where(x => x.Services.Any(x => x.Id == queryParameters.ServiceId));

    if (queryParameters.ExpectedCheckIn.HasValue)
      query = query.FilterByOperator(queryParameters.ExpectedCheckInOperator, x => x.ExpectedCheckIn, queryParameters.ExpectedCheckIn);

    if (queryParameters.ExpectedCheckOut.HasValue)
      query = query.FilterByOperator(queryParameters.ExpectedCheckOutOperator, x => x.ExpectedCheckOut, queryParameters.ExpectedCheckOut);

    query = query.BaseQuery(queryParameters);

    return await query.Select(x => new GetReservation(
        x.Id,
        x.DailyRate,
        x.ExpectedTimeHosted,
        x.ExpectedCheckIn,
        x.ExpectedCheckOut,
        x.TimeHosted,
        x.CheckIn,
        x.CheckOut,
        x.Status,
        x.Capacity,
        x.RoomId,
        x.CustomerId,
        x.InvoiceId))
     .ToListAsync();
  }

  public async Task<Reservation?> GetReservationIncludesServices(Guid id)
  {
    return await _context.Reservations
      .Where(x => x.Id == id)
      .Include(x => x.Services)
      .FirstOrDefaultAsync();
  }

  public async Task<Reservation?> GetReservationIncludesCustomer(Guid id)
  {
    return await _context.Reservations
      .Include(x => x.Customer)
      .FirstOrDefaultAsync(x => x.Id == id);
  }

  public async Task<Reservation?> GetReservationIncludesAll(Guid id)
  {
    return await _context.Reservations
      .Include(x => x.Customer)
      .Include(x => x.Services)
      .Include(x => x.Room)
      .FirstOrDefaultAsync(x => x.Id == id);
  }
}