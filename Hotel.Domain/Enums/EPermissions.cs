namespace Hotel.Domain.Enums;


public enum EPermissions
{
  //Admins
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

  //Customers
  EditCustomer,
  DeleteCustomer,

  //Employees
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
    
  //Responsabilities
  GetResponsabilities,
  GetResponsability,
  CreateResponsability,
  EditResponsability,
  DeleteResponsability,
  
  //RoomInvoices
  DeleteRoomInvoice,
  GetRoomInvoices,
  GetRoomInvoice,

  //Reservations
  GetReservations,
  DeleteReservation,
  UpdateReservationCheckout,
  UpdateReservationCheckIn,
  AddServiceToReservation,
  RemoveServiceFromReservation,
    
  //Categories
  CreateCategory,
  EditCategory,
  DeleteCategory,
  
  //Reports
  GetReports,
  GetReport,
  EditReport,
  CreateReport,
  FinishReport,
  
  //Rooms
  CreateRoom,
  EditRoom,
  DeleteRoom,
  AddServiceToRoom,
  RemoveServiceToRoom,
  UpdateRoomNumber,
  UpdateRoomCapacity,
  UpdateRoomCategory,
  UpdateRoomPrice

}

