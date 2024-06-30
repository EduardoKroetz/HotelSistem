using Hotel.Domain.DTOs.Base.User;

namespace Hotel.Domain.DTOs.AdminDTOs;
public class AdminQueryParameters : UserQueryParameters
{
    public bool? IsRootAdmin { get; set; }
    public Guid? PermissionId { get; set; }
}

