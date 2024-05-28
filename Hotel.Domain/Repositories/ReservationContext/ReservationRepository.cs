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
      .Include(x => x.Customers)
      .Include(x => x.Services)
      .Select(x => new GetReservation(x.Id,
        x.DailyRate,
        x.HostedDays,
        x.CheckIn,
        x.CheckOut,
        x.Status,
        x.Capacity,
        x.RoomId,
        new List<GetUser>(
          x.Customers.Select(
            c => new GetUser(c.Id, c.Name.FirstName, c.Name.LastName, c.Email.Address, c.Phone.Number, c.Gender,c.DateOfBirth, c.Address,c.CreatedAt)
        )),
        x.InvoiceId,
        x.Services))
      .FirstOrDefaultAsync();

  }

  public async Task<IEnumerable<GetReservationCollection>> GetAsync(ReservationQueryParameters queryParameters)
  {
    var query = _context.Reservations.AsQueryable();

    if (queryParameters.HostedDays.HasValue)
      query = query.FilterByOperator(queryParameters.HostedDaysOperator,x => x.HostedDays,queryParameters.HostedDays);

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
      query = query.Where(x => x.Customers.Any(x => x.Id == queryParameters.CustomerId));

    if (queryParameters.RoomId.HasValue)
      query = query.Where(x => x.RoomId == queryParameters.RoomId);

    if (queryParameters.InvoiceId.HasValue)
      query = query.Where(x => x.InvoiceId == queryParameters.InvoiceId);

    if (queryParameters.ServiceId.HasValue)
      query = query.Where(x => x.Services.Any(x => x.Id == queryParameters.ServiceId));


    query = query.BaseQuery(queryParameters);

    return await query.Select(x => new GetReservationCollection(
        x.Id,
        x.DailyRate,
        x.HostedDays,
        x.CheckIn,
        x.CheckOut,
        x.Status,
        x.Capacity,
        x.RoomId,
        x.InvoiceId
    )).ToListAsync();
  }

  public async Task<Reservation?> GetReservationIncludeServices(Guid id)
  {
    return await _context.Reservations
      .Where(x => x.Id == id)
      .Include(x => x.Services)
      .FirstOrDefaultAsync();
  }

  public async Task<Reservation?> GetReservationIncludeCustomers(Guid id)
  {
    return await _context.Reservations
      .Where(x => x.Id == id)
      .Include(x => x.Customers)
      .FirstOrDefaultAsync();
  }
}