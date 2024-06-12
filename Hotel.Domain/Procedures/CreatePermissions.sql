CREATE PROCEDURE CreatePermissions
AS
BEGIN
    -- Verifica se a permissão já existe antes de adicioná-la para evitar duplicatas

    --Admins

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'GetAdmins')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'GetAdmins', 'Permissão para obter informações sobre administradores', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'GetAdmin')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'GetAdmin', 'Permissão para obter informações sobre um administrador específico', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'EditAdmin')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'EditAdmin', 'Permissão para editar um administrador', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'DeleteAdmin')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'DeleteAdmin', 'Permissão para excluir um administrador', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'AdminAssignPermission')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'AdminAssignPermission', 'Permissão para atribuir permissões a um administrador', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'AdminUnassignPermission')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'AdminUnassignPermission', 'Permissão para remover permissões de um administrador', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'DefaultAdminPermission')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'DefaultAdminPermission', 'Todas as permissões padrão de um administrador', 1, GETDATE());
    END;

    --Customer

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'EditCustomer')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'EditCustomer', 'Permissão para editar os campos de um cliente.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'DeleteCustomer')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'DeleteCustomer', 'Permissão para deletar um cliente.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'CreateAdmin')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'CreateAdmin', 'Permissão para criar um administrador.', 1, GETDATE());
    END;

    --Employee

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'DefaultEmployeePermission')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'DefaultEmployeePermission', 'Permissão padrão para um funcionário.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'GetEmployee')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'GetEmployee', 'Permissão para visualizar informações de um funcionário.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'GetEmployees')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'GetEmployees', 'Permissão para visualizar informações de múltiplos funcionários.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'DeleteEmployee')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'DeleteEmployee', 'Permissão para deletar um funcionário.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'EditEmployee')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'EditEmployee', 'Permissão para editar informações de um funcionário.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'CreateEmployee')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'CreateEmployee', 'Permissão para criar um funcionário.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'AssignEmployeeResponsibility')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'AssignEmployeeResponsibility', 'Permissão para atribuir responsabilidades a um funcionário.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'UnassignEmployeeResponsibility')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'UnassignEmployeeResponsibility', 'Permissão para desatribuir responsabilidades de um funcionário.', 1, GETDATE());
    END;

        IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'AssignEmployeePermission')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'AssignEmployeePermission', 'Permissão para atribuir permissões a um funcionário.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'UnassignEmployeePermission')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'UnassignEmployeePermission', 'Permissão para desatribuir permissões de um funcionário.', 1, GETDATE());
    END;

    --Responsibilities

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'GetResponsibilities')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'GetResponsibilities', 'Permissão para visualizar todas as responsabilidades.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'GetResponsibility')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'GetResponsibility', 'Permissão para visualizar uma responsabilidade específica.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'CreateResponsibility')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'CreateResponsibility', 'Permissão para criar uma nova responsabilidade.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'EditResponsibility')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'EditResponsibility', 'Permissão para editar uma responsabilidade existente.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'DeleteResponsibility')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'DeleteResponsibility', 'Permissão para deletar uma responsabilidade.', 1, GETDATE());
    END;
    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'DeleteRoomInvoice')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'DeleteRoomInvoice', 'Permissão para deletar uma fatura de quarto.', 1, GETDATE());
    END;

    --Room invoices

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'GetRoomInvoices')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'GetRoomInvoices', 'Permissão para visualizar faturas de quarto.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'GetRoomInvoice')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'GetRoomInvoice', 'Permissão para visualizar uma fatura de quarto.', 1, GETDATE());
    END;



    --Reservations

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'CreateReservation')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'CreateReservation', 'Permissão para criar uma nova reserva.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'DeleteReservation')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'DeleteReservation', 'Permissão para deletar uma reserva.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'UpdateReservationCheckout')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'UpdateReservationCheckout', 'Permissão para atualizar o checkout de uma reserva.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'UpdateReservationCheckIn')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'UpdateReservationCheckIn', 'Permissão para atualizar o check-in de uma reserva.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'AddServiceToReservation')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'AddServiceToReservation', 'Permissão para adicionar um serviço a uma reserva.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'RemoveServiceFromReservation')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'RemoveServiceFromReservation', 'Permissão para remover um serviço de uma reserva.', 1, GETDATE());
    END;

    -- Categories
    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'CreateCategory')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'CreateCategory', 'Permissão para criar uma nova categoria.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'EditCategory')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'EditCategory', 'Permissão para editar uma categoria existente.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'DeleteCategory')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'DeleteCategory', 'Permissão para deletar uma categoria.', 1, GETDATE());
    END;

    -- Reports
    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'GetReports')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'GetReports', 'Permissão para visualizar todos os relatórios.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'GetReport')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'GetReport', 'Permissão para visualizar um relatório específico.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'EditReport')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'EditReport', 'Permissão para editar um relatório.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'CreateReport')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'CreateReport', 'Permissão para criar um novo relatório.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'FinishReport')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'FinishReport', 'Permissão para finalizar um relatório.', 1, GETDATE());
    END;

    -- Rooms
    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'CreateRoom')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'CreateRoom', 'Permissão para criar um novo quarto.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'EditRoom')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'EditRoom', 'Permissão para editar um quarto existente.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'DeleteRoom')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'DeleteRoom', 'Permissão para deletar um quarto.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'AddRoomService')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'AddRoomService', 'Permissão para adicionar um serviço a um quarto.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'RemoveRoomService')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'RemoveRoomService', 'Permissão para remover um serviço de um quarto.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'UpdateRoomNumber')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'UpdateRoomNumber', 'Permissão para atualizar o número de um quarto.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'UpdateRoomCapacity')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'UpdateRoomCapacity', 'Permissão para atualizar a capacidade de um quarto.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'UpdateRoomCategory')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'UpdateRoomCategory', 'Permissão para atualizar a categoria de um quarto.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'UpdateRoomPrice')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'UpdateRoomPrice', 'Permissão para atualizar o preço de um quarto.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'EnableRoom')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'EnableRoom', 'Permissão para habilitar um quarto.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'DisableRoom')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'DisableRoom', 'Permissão para desabilitar um quarto.', 1, GETDATE());
    END;


    -- Services
    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'GetServices')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'GetServices', 'Permissão para obter todos os serviços.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'GetService')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'GetService', 'Permissão para obter um serviço por ID.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'UpdateService')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'UpdateService', 'Permissão para atualizar um serviço.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'CreateService')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'CreateService', 'Permissão para criar um novo serviço.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'DeleteService')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'DeleteService', 'Permissão para deletar um serviço.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'AssignServiceResponsibility')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'AssignServiceResponsibility', 'Permissão para atribuir uma responsabilidade a um serviço.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'UnassignServiceResponsibility')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'UnassignServiceResponsibility', 'Permissão para desatribuir uma responsabilidade de um serviço.', 1, GETDATE());
    END;
    
    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'AvailableRoomStatus')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'AvailableRoomStatus', 'Permissão para alterar o status de um cômodo para disponível.', 1, GETDATE());
    END;
  
END;




