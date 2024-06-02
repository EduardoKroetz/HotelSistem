namespace Hotel.Domain.Enums;


public enum EPermissions
{
  //Admins
  DefaultAdminPermission = 1,
  AdminAssignPermission,
  AdminUnassignPermission,
  GetAdmins,
  GetAdmin,
  DeleteAdmin,
  EditAdmin,
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
  AddRoomService,
  RemoveRoomService,
  UpdateRoomNumber,
  UpdateRoomCapacity,
  UpdateRoomCategory,
  UpdateRoomPrice,
  EnableRoom,
  DisableRoom,

  //Services
  GetServices,
  GetService,
  UpdateService,
  CreateService,
  DeleteService,
  AssignServiceResponsability,
  UnassignServiceResponsability,
}

