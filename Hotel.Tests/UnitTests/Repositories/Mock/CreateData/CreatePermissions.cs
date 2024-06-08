using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Tests.UnitTests.Repositories.Mock;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.UnitTests.Repositories.Mock.CreateData;

public class CreatePermissions
{
    public static async Task Create()
    {
        var permissions = new List<Permission>()
    {
      new("Atualizar usuário","Atualizar usuário"),
      new("Criar administrador","Criar administrador"),
      new("Buscar reservas","Buscar reservas"),
      new("Gerar fatura","Gerar fatura")
    };

        await BaseRepositoryTest.MockConnection.Context.Permissions.AddRangeAsync(permissions);
        await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

        BaseRepositoryTest.Permissions = await BaseRepositoryTest.MockConnection.Context.Permissions.ToListAsync();
    }
}
