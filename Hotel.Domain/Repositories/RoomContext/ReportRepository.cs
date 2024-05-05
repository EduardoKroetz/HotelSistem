using Hotel.Domain.Data;
using Hotel.Domain.Entities.RoomContext.ReportEntity;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Repositories;

public class ReportRepository :  GenericRepository<Report> ,IReportRepository
{
  public ReportRepository(HotelDbContext context) : base(context) {}
 
}