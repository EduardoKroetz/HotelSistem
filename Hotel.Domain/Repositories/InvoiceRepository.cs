using Hotel.Domain.Data;
using Hotel.Domain.DTOs.InvoiceDTOs;
using Hotel.Domain.Entities.InvoiceEntity;
using Hotel.Domain.Extensions;
using Hotel.Domain.Repositories.Base;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(HotelDbContext context) : base(context) { }


    public async Task<GetInvoice?> GetByIdAsync(Guid id)
    {
        return await _context
          .Invoices
          .AsNoTracking()
          .Where(x => x.Id == id)
          .Select(x => new GetInvoice(x.Id, x.Number, x.PaymentMethod, x.ReservationId, x.IssueDate, x.TotalAmount, x.Status, x.TaxInformation, x.CustomerId))
          .FirstOrDefaultAsync();

    }

    public async Task<IEnumerable<GetInvoice>> GetAsync(InvoiceQueryParameters queryParameters)
    {
        var query = _context.Invoices.AsQueryable();

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
            query = query.Where(x => x.CustomerId == queryParameters.CustomerId);

        if (queryParameters.ReservationId.HasValue)
            query = query.Where(x => x.ReservationId == queryParameters.ReservationId);

        if (queryParameters.ServiceId.HasValue)
            query = query.Where(x => x.Services.Any(x => x.Id == queryParameters.ServiceId));

        if (queryParameters.TaxInformation.HasValue)
            query = query.FilterByOperator(queryParameters.TaxInformationOperator, x => x.TaxInformation, queryParameters.TaxInformation);

        query = query.Skip(queryParameters.Skip ?? 0).Take(queryParameters.Take ?? 1);
        query = query.AsNoTracking();

        return await query.Select(x => new GetInvoice(
          x.Id,
          x.Number,
          x.PaymentMethod,
          x.ReservationId,
          x.IssueDate,
          x.TotalAmount,
          x.Status,
          x.TaxInformation,
          x.CustomerId
        )).ToListAsync();
    }
}