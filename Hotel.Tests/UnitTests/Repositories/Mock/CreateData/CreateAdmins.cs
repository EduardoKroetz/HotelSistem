using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.UnitTests.Repositories.Mock;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.UnitTests.Repositories.Mock.CreateData;

public class CreateAdmins
{
    public static async Task Create()
    {
        var admins = new List<Admin>()
    {
      new(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Pauki", "Av. Sp", 999)),
      new(new Name("Maria", "Silva"), new Email("maria.silva@example.com"), new Phone("+55 (11) 98765-1121"), "senha123", EGender.Feminine, DateTime.Now.AddYears(-25), new Address("Brazil", "Rio de Janeiro", "Rua Copacabana", 123)),
      new(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 12345-6789"), "_admin123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)),
      new(new Name("Ana", "Santos"), new Email("anasantos@example.com"), new Phone("+55 (31) 98765-4321"), "ana456", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Belo Horizonte", "Avenida Afonso Pena", 456)),
      new(new Name("Rafael", "Silveira"), new Email("rafaelsilveira@example.com"), new Phone("+55 (19) 98765-4323"), "rafa789", EGender.Masculine, DateTime.Now.AddYears(-32), new Address("Brazil", "Campinas", "Rua Barão de Jaguara", 789))
    };

        await BaseRepositoryTest.MockConnection.Context.Admins.AddRangeAsync(admins);
        await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

        BaseRepositoryTest.Admins = await BaseRepositoryTest.MockConnection.Context.Admins.ToListAsync();
    }
}
