using Hotel.Domain.DTOs.ReportDTOs;
using Hotel.Domain.Entities.ReportEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IReportRepository : IRepository<Report>, IRepositoryQuery<GetReport, ReportQueryParameters>
{
}