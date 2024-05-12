using Hotel.Domain.DTOs.RoomContext.ReportDTOs;
using Hotel.Domain.Entities.RoomContext.ReportEntity;

namespace Hotel.Domain.Repositories.Interfaces.RoomContext;

public interface IReportRepository : IRepository<Report>, IRepositoryQuery<GetReport, ReportQueryParameters>
{
}