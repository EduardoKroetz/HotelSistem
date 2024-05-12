using Hotel.Domain.DTOs.RoomContext.ReportDTOs;
using Hotel.Domain.Entities.RoomContext.ReportEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IReportRepository : IRepository<Report>, IRepositoryQuery<GetReport,ReportQueryParameters>
{
}