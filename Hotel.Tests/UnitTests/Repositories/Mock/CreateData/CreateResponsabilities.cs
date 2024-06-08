using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Enums;
using Hotel.Tests.UnitTests.Repositories.Mock;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.UnitTests.Repositories.Mock.CreateData;

public class CreateResponsabilities
{
    public static async Task Create()
    {
        var responsabilities = new List<Responsability>()
    {
      new("Secretária","Secretária",EPriority.Medium),
      new("Atender a chamadas de serviço","Atender a chamadas de serviço",EPriority.High),
      new("Assistência geral", "Assistência geral", EPriority.Medium),
      new("Cozinheiro","Cozinheiro",EPriority.High),
      new("Organizar arquivos", "Organizar arquivos", EPriority.Trivial),
      new("Atualizar registros", "Atualizar registros", EPriority.Low),
      new("Gerenciar crises", "Gerenciar crises", EPriority.Critical),
      new("Redigir documentos", "Redigir documentos", EPriority.Low),
      new("Planejar eventos", "Planejar eventos", EPriority.Trivial),
      new("Supervisionar equipe", "Supervisionar equipe", EPriority.Critical),
      new("Desenvolver estratégias de marketing", "Desenvolver estratégias de marketing", EPriority.High),
      new("Analisar dados financeiros", "Analisar dados financeiros", EPriority.Medium),
      new("Manter relacionamento com clientes", "Manter relacionamento com clientes", EPriority.Low),
      new("Treinar novos funcionários", "Treinar novos funcionários", EPriority.Critical),
      new("Gerenciar inventário", "Gerenciar inventário", EPriority.Low),
      new("Planejar a logística de entrega", "Planejar a logística de entrega", EPriority.Medium),
      new("Implementar políticas de segurança", "Implementar políticas de segurança", EPriority.Critical),
      new("Apoiar a equipe de TI", "Apoiar a equipe de TI", EPriority.Trivial),
      new("Preparar relatórios mensais", "Preparar relatórios mensais", EPriority.High),
      new("Organizar reuniões de equipe", "Organizar reuniões de equipe", EPriority.Medium)
    };

        await BaseRepositoryTest.MockConnection.Context.Responsabilities.AddRangeAsync(responsabilities);
        await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

        BaseRepositoryTest.Responsabilities = await BaseRepositoryTest.MockConnection.Context.Responsabilities.ToListAsync();
    }
}
