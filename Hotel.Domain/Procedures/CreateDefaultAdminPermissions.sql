CREATE PROCEDURE AddDefaultAdminPermissions
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
END;

