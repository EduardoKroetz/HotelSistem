CREATE PROCEDURE AddDefaultAdminPermissions
AS
BEGIN
    -- Verifica se a permissão já existe antes de adicioná-la para evitar duplicatas
    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'GetAdmins')
    BEGIN
        INSERT INTO Permissions (Name, Description)
        VALUES ('GetAdmins', 'Permissão para obter informações sobre administradores');
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'GetAdmin')
    BEGIN
        INSERT INTO Permissions (Name, Description)
        VALUES ('GetAdmin', 'Permissão para obter informações sobre um administrador específico');
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'EditAdmin')
    BEGIN
        INSERT INTO Permissions (Name, Description)
        VALUES ('EditAdmin', 'Permissão para editar um administrador');
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'DeleteAdmin')
    BEGIN
        INSERT INTO Permissions (Name, Description)
        VALUES ('DeleteAdmin', 'Permissão para excluir um administrador');
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'Admin-AssignPermission')
    BEGIN
        INSERT INTO Permissions (Name, Description)
        VALUES ('Admin-AssignPermission', 'Permissão para atribuir permissões a um administrador');
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'Admin-UnassignPermission')
    BEGIN
        INSERT INTO Permissions (Name, Description)
        VALUES ('Admin-UnassignPermission', 'Permissão para remover permissões de um administrador');
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'Admin-EditName')
    BEGIN
        INSERT INTO Permissions (Name, Description)
        VALUES ('Admin-EditName', 'Permissão para editar o nome de um administrador');
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'Admin-EditEmail')
    BEGIN
        INSERT INTO Permissions (Name, Description)
        VALUES ('Admin-EditEmail', 'Permissão para editar o e-mail de um administrador');
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'Admin-EditPhone')
    BEGIN
        INSERT INTO Permissions (Name, Description)
        VALUES ('Admin-EditPhone', 'Permissão para editar o telefone de um administrador');
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'Admin-EditAddress')
    BEGIN
        INSERT INTO Permissions (Name, Description)
        VALUES ('Admin-EditAddress', 'Permissão para editar o endereço de um administrador');
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'Admin-EditGender')
    BEGIN
        INSERT INTO Permissions (Name, Description)
        VALUES ('Admin-EditGender', 'Permissão para editar o gênero de um administrador');
    END;

    IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Name = 'Admin-EditDateOfBirth')
    BEGIN
        INSERT INTO Permissions (Name, Description)
        VALUES ('Admin-EditDateOfBirth', 'Permissão para editar a data de nascimento de um administrador');
    END;
END;
