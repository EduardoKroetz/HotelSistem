using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.Mock.CreateData;

public class CreateEmployees
{
  public static async Task Create()
  {
    await BaseRepositoryTest.MockConnection.Context.Employees.AddRangeAsync([
    new Employee(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Pauki", "Av. Sp", 999),900),
    new Employee(new Name("Maria", "Silva"), new Email("maria.silva@example.com"), new Phone("+55 (11) 98765-4321"), "senha123", EGender.Feminine, DateTime.Now.AddYears(-25), new Address("Brazil", "Rio de Janeiro", "Rua Copacabana", 123),1800),
    new Employee(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 12345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3),2000),
    new Employee(new Name("Ana", "Santos"), new Email("anasantos@example.com"), new Phone("+55 (31) 98765-4321"), "ana456", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Belo Horizonte", "Avenida Afonso Pena", 456),1400),
    new Employee(new Name("Rafael", "Silveira"), new Email("rafaelsilveira@example.com"), new Phone("+55 (19) 98765-4321"), "rafa789", EGender.Masculine, DateTime.Now.AddYears(-32), new Address("Brazil", "Campinas", "Rua Barão de Jaguara", 789),2300),
    new Employee(new Name("Ana", "Pereira"), new Email("anapereira@example.com"), new Phone("+55 (11) 91234-5678"), "pWd98dkL", EGender.Feminine, DateTime.Now.AddYears(-25), new Address("Brazil", "Rio de Janeiro", "Rua das Flores", 123), 1200m),
    new Employee(new Name("Carlos", "Santos"), new Email("carlossantos@example.com"), new Phone("+55 (21) 99876-5432"), "kLm78xYz", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Belo Horizonte", "Av. Amazonas", 456), 1500m),
    new Employee(new Name("Mariana", "Oliveira"), new Email("marianaoliveira@example.com"), new Phone("+55 (31) 98765-4321"), "qWe45rTy", EGender.Feminine, DateTime.Now.AddYears(-22), new Address("Brazil", "Porto Alegre", "Rua da Praia", 789), 1100m),
    new Employee(new Name("João", "Lima"), new Email("joaolima@example.com"), new Phone("+55 (51) 91234-8765"), "rTg67uIo", EGender.Masculine, DateTime.Now.AddYears(-28), new Address("Brazil", "Curitiba", "Rua XV de Novembro", 1011), 1400m),
    new Employee(new Name("Fernanda", "Costa"), new Email("fernandacosta@example.com"), new Phone("+55 (41) 99876-4321"), "bNm89vCx", EGender.Feminine, DateTime.Now.AddYears(-26), new Address("Brazil", "Florianópolis", "Av. Beira Mar", 1213), 1300m),
    new Employee(new Name("Paulo", "Mendes"), new Email("paulomendes@example.com"), new Phone("+55 (61) 91234-5678"), "uIo90pLp", EGender.Masculine, DateTime.Now.AddYears(-32), new Address("Brazil", "Brasília", "Eixo Monumental", 1415), 1600m),
    new Employee(new Name("Larissa", "Fernandes"), new Email("larissafernandes@example.com"), new Phone("+55 (71) 98765-4321"), "zAs34fGh", EGender.Feminine, DateTime.Now.AddYears(-24), new Address("Brazil", "Salvador", "Av. Sete de Setembro", 1617), 1150m),
    new Employee(new Name("Ricardo", "Almeida"), new Email("ricardoalmeida@example.com"), new Phone("+55 (81) 91234-8765"), "yXw21qPe", EGender.Masculine, DateTime.Now.AddYears(-29), new Address("Brazil", "Recife", "Rua da Aurora", 1819), 1450m),
    new Employee(new Name("Sofia", "Martins"), new Email("sofiamartins@example.com"), new Phone("+55 (85) 99876-5432"), "oPl65nBm", EGender.Feminine, DateTime.Now.AddYears(-27), new Address("Brazil", "Fortaleza", "Av. Beira Mar", 2021), 1350m),
  ]);
    await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

    BaseRepositoryTest.Employees = await BaseRepositoryTest.MockConnection.Context.Employees.ToListAsync();
  }
}
