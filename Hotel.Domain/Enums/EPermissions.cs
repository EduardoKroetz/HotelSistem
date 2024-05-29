namespace Hotel.Domain.Enums;


public enum EPermissions
{
  DefaultAdminPermission = 1,
  AdminAssignPermission,
  AdminUnassignPermission,
  AdminEditGender,
  GetAdmins,
  GetAdmin,
  DeleteAdmin,
  AdminEditDateOfBirth,
  EditAdmin,
  AdminEditEmail,
  AdminEditAddress,
  AdminEditPhone,
  AdminEditName,
  CreateAdmin,

  EditCustomer,
  DeleteCustomer,

  DefaultEmployeePermission,
  GetEmployee,
  GetEmployees,
  DeleteEmployee,
  EditEmployee,
  CreateEmployee,
  AssignEmployeeResponsability,
  UnassignEmployeeResponsability,
  AssignEmployeePermission,
  UnassignEmployeePermission,

  DeleteRoomInvoice,
  GetRoomInvoices,
}

