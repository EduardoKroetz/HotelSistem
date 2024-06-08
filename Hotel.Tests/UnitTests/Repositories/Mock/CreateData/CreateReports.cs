using Hotel.Domain.Entities.RoomContext.ReportEntity;
using Hotel.Domain.Enums;
using Hotel.Tests.UnitTests.Repositories.Mock;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.UnitTests.Repositories.Mock.CreateData;

public class CreateReports
{
    public static async Task Create()
    {
        var reports = new List<Report>()
  {
    new("Vazamento de água no quarto 21","Vazamento de água no quarto 21 devido a encanação",EPriority.High,BaseRepositoryTest.Employees[0],"Chamar o encanador"),
    new("Computador não liga", "Computador no escritório não está ligando, possivelmente problema na fonte de alimentação", EPriority.Critical, BaseRepositoryTest.Employees[1], "Verificar a fonte de alimentação e os cabos"),
    new("Ar condicionado com defeito", "Ar condicionado da sala de reuniões está fazendo barulho estranho", EPriority.Medium, BaseRepositoryTest.Employees[2], "Chamar o técnico de manutenção"),
    new("Falta de material de escritório", "Estoque de papel para impressão está baixo", EPriority.Low, BaseRepositoryTest.Employees[3], "Requisitar mais papel para o almoxarifado"),
    new("Falha na rede Wi-Fi", "Rede Wi-Fi está instável no segundo andar", EPriority.High, BaseRepositoryTest.Employees[4], "Verificar roteadores e pontos de acesso"),
    new("Lâmpada queimada", "Lâmpada do corredor principal está queimada", EPriority.Trivial, BaseRepositoryTest.Employees[5], "Substituir a lâmpada queimada"),
    new("Cadeira quebrada", "Cadeira da sala de espera está quebrada", EPriority.Low, BaseRepositoryTest.Employees[6], "Reparar ou substituir a cadeira"),
    new("Sistema de alarme disparado", "Sistema de alarme disparou sem motivo aparente", EPriority.Critical, BaseRepositoryTest.Employees[7], "Chamar a equipe de segurança para verificar"),
    new("Telefone com problemas", "Telefone do setor de atendimento ao cliente não está funcionando", EPriority.Medium, BaseRepositoryTest.Employees[8], "Verificar as conexões do telefone"),
    new("Janela com vazamento", "Janela da sala de conferências está com vazamento de água", EPriority.High, BaseRepositoryTest.Employees[9], "Solicitar reparo na vedação da janela"),
    new("Problema na impressora", "Impressora do departamento financeiro está atolando papel", EPriority.Medium, BaseRepositoryTest.Employees[10], "Realizar manutenção na impressora")
  };

        await BaseRepositoryTest.MockConnection.Context.Reports.AddRangeAsync(reports);
        await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

        BaseRepositoryTest.Reports = await BaseRepositoryTest.MockConnection.Context.Reports.ToListAsync();
    }

}
