using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Entities.PermissionEntity;
using Hotel.Domain.Entities.ReportEntity;
using Hotel.Domain.Entities.ResponsibilityEntity;

namespace Hotel.Domain.Entities.Interfaces;
public interface IEmployee : IUser
{
    decimal? Salary { get; }
    ICollection<Responsibility> Responsibilities { get; }
    ICollection<Report> Reports { get; }
    ICollection<Permission> Permissions { get; }

}
