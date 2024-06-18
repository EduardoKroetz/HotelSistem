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
    AssignEmployeeResponsibility,
    UnassignEmployeeResponsibility,
    AssignEmployeePermission,
    UnassignEmployeePermission,

    //Responsibilities
    GetResponsibilities,
    GetResponsibility,
    CreateResponsibility,
    EditResponsibility,
    DeleteResponsibility,

    //Invoices
    DeleteInvoice,
    GetInvoices,
    GetInvoice,

    //Reservations
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
    AvailableRoomStatus,

    //Services
    GetServices,
    GetService,
    UpdateService,
    CreateService,
    DeleteService,
    AssignServiceResponsibility,
    UnassignServiceResponsibility,
}

