using Hotel.Domain.Entities.CategoryEntity;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.UnitTests.Repositories.Mock.CreateData;

public class CreateCategories
{
    public static async Task Create()
    {
        var categories = new List<Category>()
        {
            new("Quartos médios", "Quartos de médio para uma hospedagem temporária", 40),
            new("Quartos de luxo", "Quartos espaçosos e luxuosos com comodidades premium.", 120),
            new("Quartos padrão", "Quartos confortáveis e bem equipados para uma estadia agradável.", 100),
            new("Suítes executivas", "Suítes espaçosas com áreas de estar separadas e serviços exclusivos.", 200),
            new("Quartos familiares", "Quartos amplos e confortáveis para acomodar toda a família.", 130),
            new("Suítes presidenciais", "Suítes de alto padrão com vistas deslumbrantes e serviços personalizados.", 300)
        };

        await BaseRepositoryTest.MockConnection.Context.Categories.AddRangeAsync(categories);
        await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

        BaseRepositoryTest.Categories = await BaseRepositoryTest.MockConnection.Context.Categories.ToListAsync();
    }

}
