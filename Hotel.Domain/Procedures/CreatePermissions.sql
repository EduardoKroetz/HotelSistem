CREATE PROCEDURE CreatePermissions
AS
BEGIN
    -- Verifica se a permissão já existe antes de adicioná-la para evitar duplicatas
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

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'AdminEditName')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'AdminEditName', 'Permissão para editar o nome de um administrador', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'AdminEditEmail')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'AdminEditEmail', 'Permissão para editar o e-mail de um administrador', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'AdminEditPhone')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'AdminEditPhone', 'Permissão para editar o telefone de um administrador', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'AdminEditAddress')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'AdminEditAddress', 'Permissão para editar o endereço de um administrador', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'AdminEditGender')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'AdminEditGender', 'Permissão para editar o gênero de um administrador', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'AdminEditDateOfBirth')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'AdminEditDateOfBirth', 'Permissão para editar a data de nascimento de um administrador', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'DefaultAdminPermission')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'DefaultAdminPermission', 'Todas as permissões padrão de um administrador', 1, GETDATE());
    END;

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

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'AssignEmployeeResponsability')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'AssignEmployeeResponsability', 'Permissão para atribuir responsabilidades a um funcionário.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'UnassignEmployeeResponsability')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'UnassignEmployeeResponsability', 'Permissão para desatribuir responsabilidades de um funcionário.', 1, GETDATE());
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

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'GetResponsabilities')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'GetResponsabilities', 'Permissão para visualizar todas as responsabilidades.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'GetResponsability')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'GetResponsability', 'Permissão para visualizar uma responsabilidade específica.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'CreateResponsability')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'CreateResponsability', 'Permissão para criar uma nova responsabilidade.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'EditResponsability')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'EditResponsability', 'Permissão para editar uma responsabilidade existente.', 1, GETDATE());
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'DeleteResponsability')
    BEGIN
        INSERT INTO Permissions (ID, Name, Description, IsActive, CreatedAt)
        VALUES (NEWID(), 'DeleteResponsability', 'Permissão para deletar uma responsabilidade.', 1, GETDATE());
    END;


END;


