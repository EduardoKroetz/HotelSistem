using Hotel.Domain.Data;
using Hotel.Domain.Entities.PermissionEntity;

public class SeedPermissions
{
    private readonly HotelDbContext _context;

    public SeedPermissions(HotelDbContext context)
    {
        _context = context;
    }

    public async Task CreatePermissionsAsync()
    {
        var permissionsToAdd = new List<Permission>
        {
            new Permission("GetAdmins", "Permissão para obter informações sobre administradores"),
            new Permission("GetAdmin", "Permissão para obter informações sobre um administrador específico"),
            new Permission("EditAdmin", "Permissão para editar um administrador"),
            new Permission("DeleteAdmin", "Permissão para excluir um administrador"),
            new Permission("AdminAssignPermission", "Permissão para atribuir permissões a um administrador"),
            new Permission("AdminUnassignPermission", "Permissão para remover permissões de um administrador"),
            new Permission("DefaultAdminPermission", "Todas as permissões padrão de um administrador"),
            new Permission("EditCustomer", "Permissão para editar os campos de um cliente"),
            new Permission("DeleteCustomer", "Permissão para deletar um cliente"),
            new Permission("CreateAdmin", "Permissão para criar um administrador"),
            new Permission("DefaultEmployeePermission", "Permissão padrão para um funcionário"),
            new Permission("GetEmployee", "Permissão para visualizar informações de um funcionário"),
            new Permission("GetEmployees", "Permissão para visualizar informações de múltiplos funcionários"),
            new Permission("DeleteEmployee", "Permissão para deletar um funcionário"),
            new Permission("EditEmployee", "Permissão para editar informações de um funcionário"),
            new Permission("CreateEmployee", "Permissão para criar um funcionário"),
            new Permission("AssignEmployeeResponsibility", "Permissão para atribuir responsabilidades a um funcionário"),
            new Permission("UnassignEmployeeResponsibility", "Permissão para desatribuir responsabilidades de um funcionário"),
            new Permission("AssignEmployeePermission", "Permissão para atribuir permissões a um funcionário"),
            new Permission("UnassignEmployeePermission", "Permissão para desatribuir permissões de um funcionário"),
            new Permission("GetResponsibilities", "Permissão para visualizar todas as responsabilidades"),
            new Permission("GetResponsibility", "Permissão para visualizar uma responsabilidade específica"),
            new Permission("CreateResponsibility", "Permissão para criar uma nova responsabilidade"),
            new Permission("EditResponsibility", "Permissão para editar uma responsabilidade existente"),
            new Permission("DeleteResponsibility", "Permissão para deletar uma responsabilidade"),
            new Permission("DeleteInvoice", "Permissão para deletar uma fatura de uma hospedagem"),
            new Permission("GetInvoices", "Permissão para visualizar faturas de uma hospedagem"),
            new Permission("GetInvoice", "Permissão para visualizar uma fatura de uma hospedagem"),
            new Permission("DeleteReservation", "Permissão para deletar uma reserva"),
            new Permission("UpdateReservationCheckout", "Permissão para atualizar o checkout de uma reserva"),
            new Permission("UpdateReservationCheckIn", "Permissão para atualizar o check-in de uma reserva"),
            new Permission("AddServiceToReservation", "Permissão para adicionar um serviço a uma reserva"),
            new Permission("RemoveServiceFromReservation", "Permissão para remover um serviço de uma reserva"),
            new Permission("CreateCategory", "Permissão para criar uma nova categoria"),
            new Permission("EditCategory", "Permissão para editar uma categoria existente"),
            new Permission("DeleteCategory", "Permissão para deletar uma categoria"),
            new Permission("GetReports", "Permissão para visualizar todos os relatórios"),
            new Permission("GetReport", "Permissão para visualizar um relatório específico"),
            new Permission("EditReport", "Permissão para editar um relatório"),
            new Permission("CreateReport", "Permissão para criar um novo relatório"),
            new Permission("FinishReport", "Permissão para finalizar um relatório"),
            new Permission("CreateRoom", "Permissão para criar um novo uma hospedagem"),
            new Permission("EditRoom", "Permissão para editar um uma hospedagem existente"),
            new Permission("DeleteRoom", "Permissão para deletar um uma hospedagem"),
            new Permission("AddRoomService", "Permissão para adicionar um serviço a um uma hospedagem"),
            new Permission("RemoveRoomService", "Permissão para remover um serviço de um uma hospedagem"),
            new Permission("UpdateRoomName", "Permissão para atualizar o nome de um uma hospedagem"),
            new Permission("UpdateRoomNumber", "Permissão para atualizar o número de um uma hospedagem"),
            new Permission("UpdateRoomCapacity", "Permissão para atualizar a capacidade de um uma hospedagem"),
            new Permission("UpdateRoomCategory", "Permissão para atualizar a categoria de um uma hospedagem"),
            new Permission("UpdateRoomPrice", "Permissão para atualizar o preço de um uma hospedagem"),
            new Permission("EnableRoom", "Permissão para habilitar um uma hospedagem"),
            new Permission("DisableRoom", "Permissão para desabilitar um uma hospedagem"),
            new Permission("GetServices", "Permissão para obter todos os serviços"),
            new Permission("GetService", "Permissão para obter um serviço por ID"),
            new Permission("UpdateService", "Permissão para atualizar um serviço"),
            new Permission("CreateService", "Permissão para criar um novo serviço"),
            new Permission("DeleteService", "Permissão para deletar um serviço"),
            new Permission("AssignServiceResponsibility", "Permissão para atribuir uma responsabilidade a um serviço"),
            new Permission("UnassignServiceResponsibility", "Permissão para desatribuir uma responsabilidade de um serviço"),
            new Permission("AvailableRoomStatus", "Permissão para alterar o status de um cômodo para disponível"),
        };

        foreach (var permission in permissionsToAdd)
        {
            if (!_context.Permissions.Any(p => p.Name == permission.Name))
            {
                await _context.Permissions.AddAsync(permission);
            }
        }

        await _context.SaveChangesAsync();

        var permissions = _context.Permissions.ToList();
    }

}
