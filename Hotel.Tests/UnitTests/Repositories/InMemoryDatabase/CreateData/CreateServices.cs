using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.UnitTests.Repositories.Mock.CreateData;

public class CreateServices
{
    public static async Task Create()
    {
        var services = new List<Service>
        {
            new("Limpeza","Limpeza", 30m, EPriority.Medium, 30),
            new("Manutenção","Manutenção", 40m, EPriority.High, 45),
            new("Café da Manhã"," Manhã",20m, EPriority.Low, 60),
            new("Wi-Fi", "Wi-Fi",10m, EPriority.Trivial, 1),
            new("Serviço de Quarto", "Serviço de Quarto",50m, EPriority.Critical, 120),
            new("Recepção","Recepção", 5, EPriority.Low, 5),
            new("Translado","Translado", 50.5m, EPriority.Medium, 25),
            new("Lavanderia","Lavanderia", 35.75m, EPriority.High, 60),
            new("Jantar","Jantar", 70, EPriority.Critical, 90),
            new("Piscina","Piscina", 10, EPriority.Trivial, 120),
            new("Spa","Spa", 50.00m, EPriority.Critical, 90),
        };

        services[0].AddResponsibility(BaseRepositoryTest.Responsibilities[0]);

        await BaseRepositoryTest.MockConnection.Context.Services.AddRangeAsync(services);
        await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

        BaseRepositoryTest.Services = await BaseRepositoryTest.MockConnection.Context.Services.ToListAsync();
    }
}
