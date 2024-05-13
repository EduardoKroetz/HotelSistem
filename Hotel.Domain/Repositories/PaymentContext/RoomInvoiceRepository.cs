using Hotel.Domain.Data;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Extensions;
using Hotel.Domain.Repositories.Interfaces.PaymentContext;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories.PaymentContext;

public class RoomInvoiceRepository : GenericRepository<RoomInvoice>, IRoomInvoiceRepository
{
  public RoomInvoiceRepository(HotelDbContext context) : base(context) { }


  public async Task<GetRoomInvoice?> GetByIdAsync(Guid id)
  {
    return await _context
      .RoomInvoices
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Select(x => new GetRoomInvoice(x.Id,x.Number, x.PaymentMethod, x.ReservationId, x.IssueDate, x.TotalAmount, x.Status, x.TaxInformation))
      .FirstOrDefaultAsync();

  }

  public async Task<IEnumerable<GetRoomInvoice>> GetAsync(RoomInvoiceQueryParameters queryParameters)
  {
    var query = _context.RoomInvoices.AsQueryable();

    if (queryParameters.Number != null)
      query = query.Where(x => x.Number.Contains(queryParameters.Number));

    if (queryParameters.IssueDate.HasValue)
      query = query.FilterByOperator(queryParameters.IssueDateOperator, x => x.IssueDate, queryParameters.IssueDate);

    if (queryParameters.TotalAmount.HasValue)
      query = query.FilterByOperator(queryParameters.TotalAmountOperator, x => x.TotalAmount, queryParameters.TotalAmount);

    if (queryParameters.Status.HasValue)
      query = query.Where(x => x.Status == queryParameters.Status);

    if (queryParameters.PaymentMethod.HasValue)
      query = query.Where(x => x.PaymentMethod == queryParameters.PaymentMethod);

    if (queryParameters.CustomerId.HasValue)
      query = query.Where(x => x.Customers.Any(x => x.Id == queryParameters.CustomerId));

    if (queryParameters.ReservationId.HasValue)
      query = query.Where(x => x.ReservationId == queryParameters.ReservationId);

    if (queryParameters.ServiceId.HasValue)
      query = query.Where(x => x.Services.Any(x => x.Id == queryParameters.ServiceId));

    if (queryParameters.TaxInformation.HasValue)
      query = query.FilterByOperator(queryParameters.TaxInformationOperator, x => x.TaxInformation, queryParameters.TaxInformation);

    query = query.BaseQuery(queryParameters);

    return await query.Select(x => new GetRoomInvoice(
      x.Id,
      x.Number,
      x.PaymentMethod,
      x.ReservationId,
      x.IssueDate,
      x.TotalAmount,
      x.Status,
      x.TaxInformation
    )).ToListAsync();
  }
}