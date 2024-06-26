using Hotel.Domain.Data;
using Hotel.Domain.DTOs.InvoiceDTOs;
using Hotel.Domain.DTOs.ServiceDTOs;
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
          .Include(x => x.Services)
          .Select(x => 
            new GetInvoice(
                x.Id,
                x.PaymentMethod, 
                x.ReservationId,
                x.TotalAmount, 
                x.CustomerId, 
                x.Services.Select(s => 
                    new GetService(s.Id, s.Name, s.Price, s.Priority, s.IsActive, s.TimeInMinutes, s.CreatedAt))))
          .FirstOrDefaultAsync();

    }

    public async Task<IEnumerable<GetInvoice>> GetAsync(InvoiceQueryParameters queryParameters)
    {
        var query = _context.Invoices.AsQueryable();

        if (queryParameters.PaymentMethod != null)
            query = query.Where(x => x.PaymentMethod == queryParameters.PaymentMethod);

        if (queryParameters.CustomerId.HasValue)
            query = query.Where(x => x.CustomerId == queryParameters.CustomerId);

        if (queryParameters.ReservationId.HasValue)
            query = query.Where(x => x.ReservationId == queryParameters.ReservationId);

        if (queryParameters.ServiceId.HasValue)
            query = query.Where(x => x.Services.Any(x => x.Id == queryParameters.ServiceId));

        if (queryParameters.TotalAmount.HasValue)
            query = query.FilterByOperator(queryParameters.TotalAmountOperator, x => x.TotalAmount, queryParameters.TotalAmount);

        query = query.Skip(queryParameters.Skip ?? 0).Take(queryParameters.Take ?? 1);
        query = query.AsNoTracking();

        return await query.Select(x => new GetInvoice(
            x.Id,
            x.PaymentMethod,
            x.ReservationId,
            x.TotalAmount,
            x.CustomerId,
            x.Services.Select(s =>
                new GetService(s.Id, s.Name, s.Price, s.Priority, s.IsActive, s.TimeInMinutes, s.CreatedAt))
        )).ToListAsync();
    }
}