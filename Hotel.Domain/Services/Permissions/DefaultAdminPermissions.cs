namespace Hotel.Domain.Services.Permissions;

public class DefaultAdminPermissions
{
  public static List<string> Permissions { get; set; } = 
  [
    "GetAdmins",
    "GetAdmin",
    "EditAdmin",
    "DeleteAdmin",
    "Admin-AssignPermission",
    "Admin-UnassignPermission",
    "Admin-EditName",
    "Admin-EditEmail",
    "Admin-EditPhone",
    "Admin-EditAddress",
    "Admin-EditGender",
    "Admin-EditDateOfBirth"
  ];
}
