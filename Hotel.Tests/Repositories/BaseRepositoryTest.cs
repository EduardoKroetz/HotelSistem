using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.CategoryEntity;
using Hotel.Domain.Entities.RoomContext.ReportEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;


namespace Hotel.Tests.Repositories;

public class BaseRepositoryTest
{
  public static ConfigMockConnection MockConnection = null!;
  public static List<Room> Rooms { get; set; } = [];
  public static List<Reservation> Reservations { get; set; } = [];
  public static List<Admin> Admins { get; set; } = [];
  public static List<Category> Categories { get; set; } = [];
  public static List<Customer> Customers { get; set; } = [];
  public static List<Employee> Employees { get; set; } = [];
  public static List<Service> Services { get; set; } = [];
  public static List<Permission> Permissions { get; set; } = [];
  public static List<Feedback> Feedbacks { get; set; } = [];
  public static List<Responsability> Responsabilities { get; set; } = [];
  public static List<RoomInvoice> RoomInvoices { get; set; } = [];
  public static List<Report> Reports { get; set; } = [];

  public static async Task Startup()
  {
    MockConnection = await GenericRepositoryTest.InitializeMockConnection();

    await CreateAdmins();
    await CreatePermissions();
    await CreateCustomers();

    await CreateEmployees();
    await CreateResponsabilities();
    await CreateReports();

    await CreateCategories();
    await CreateRooms();

    await CreateServices();

    await CreateReservations();
    await CreateRoomInvoices();

    await CreateFeedbacks();

  } 

  public static async Task Cleanup()
  {

    MockConnection.Context.Admins.RemoveRange(await MockConnection.Context.Admins.ToListAsync());
    MockConnection.Context.Employees.RemoveRange(await MockConnection.Context.Employees.ToListAsync());
    MockConnection.Context.Reports.RemoveRange(await MockConnection.Context.Reports.ToListAsync());
    MockConnection.Context.RoomInvoices.RemoveRange(await MockConnection.Context.RoomInvoices.ToListAsync());
    MockConnection.Context.Reservations.RemoveRange(await MockConnection.Context.Reservations.ToListAsync());
    MockConnection.Context.Customers.RemoveRange(await MockConnection.Context.Customers.ToListAsync());
    MockConnection.Context.Services.RemoveRange(await MockConnection.Context.Services.ToListAsync());
    MockConnection.Context.Responsabilities.RemoveRange(await MockConnection.Context.Responsabilities.ToListAsync());
    MockConnection.Context.Feedbacks.RemoveRange(await MockConnection.Context.Feedbacks.ToListAsync());
    MockConnection.Context.Permissions.RemoveRange(await MockConnection.Context.Permissions.ToListAsync());
    MockConnection.Context.Rooms.RemoveRange(await MockConnection.Context.Rooms.ToListAsync());
    MockConnection.Context.Categories.RemoveRange(await MockConnection.Context.Categories.ToListAsync());

    await MockConnection.Context.SaveChangesAsync();
    MockConnection?.Dispose();
  }

  public static async Task CreateAdmins()
  {
    var admins = new List<Admin>()
    {
      new(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Pauki", "Av. Sp", 999)),
      new(new Name("Maria", "Silva"), new Email("maria.silva@example.com"), new Phone("+55 (11) 98765-4321"), "senha123", EGender.Feminine, DateTime.Now.AddYears(-25), new Address("Brazil", "Rio de Janeiro", "Rua Copacabana", 123)),
      new(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 12345-6789"), "_admin123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)),
      new(new Name("Ana", "Santos"), new Email("anasantos@example.com"), new Phone("+55 (31) 98765-4321"), "ana456", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Belo Horizonte", "Avenida Afonso Pena", 456)),
      new(new Name("Rafael", "Silveira"), new Email("rafaelsilveira@example.com"), new Phone("+55 (19) 98765-4321"), "rafa789", EGender.Masculine, DateTime.Now.AddYears(-32), new Address("Brazil", "Campinas", "Rua Barão de Jaguara", 789))
    };

    await MockConnection.Context.Admins.AddRangeAsync(admins);
    await MockConnection.Context.SaveChangesAsync();

    Admins = await MockConnection.Context.Admins.ToListAsync();
  }

  public static async Task CreatePermissions()
  {
    var permissions = new List<Permission>()
    {
      new("Atualizar usuário","Atualizar usuário"),
      new("Criar administrador","Criar administrador"),
      new("Buscar reservas","Buscar reservas"),
      new("Gerar fatura","Gerar fatura")
    };

    await MockConnection.Context.Permissions.AddRangeAsync(permissions);
    await MockConnection.Context.SaveChangesAsync();

    Permissions = await MockConnection.Context.Permissions.ToListAsync();
  }

  public static async Task CreateCustomers()
  {
    var customers = new List<Customer>()
    {
      new(new Name("João", "Figereido"), new Email("joaofigeir@example.com"), new Phone("+55 (19) 98465-4311"), "a124sd", EGender.Masculine, DateTime.Now.AddYears(-15), new Address("Brazil", "RS", "Av. Campo", 999)),
      new(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Pauki", "Av. Sp", 999)),
      new(new Name("Maria", "Silva"), new Email("maria.silva@example.com"), new Phone("+55 (11) 98765-4321"), "senha123", EGender.Feminine, DateTime.Now.AddYears(-25), new Address("Brazil", "Rio de Janeiro", "Rua Copacabana", 123)),
      new(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 12345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)),
      new(new Name("Ana", "Santos"), new Email("anasantos@example.com"), new Phone("+55 (31) 98765-4321"), "ana456", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Belo Horizonte", "Avenida Afonso Pena", 456)),
      new(new Name("Rafael", "Silveira"), new Email("rafaelsilveira@example.com"), new Phone("+55 (19) 98765-4321"), "rafa789", EGender.Masculine, DateTime.Now.AddYears(-32), new Address("Brazil", "Campinas", "Rua Barão de Jaguara", 789))
    };

    await MockConnection.Context.Customers.AddRangeAsync(customers);

    await MockConnection.Context.SaveChangesAsync();

    Customers = await MockConnection.Context.Customers.ToListAsync();

  }

  public static async Task CreateFeedbacks()
  {
    var feedbacks = new List<Feedback>()
    {
      new("O serviço do quarto estava ótimo", 6, Customers[0].Id, Reservations[0].Id, Reservations[0].RoomId),
      new("Excelente atendimento na recepção!", 5, Customers[1].Id, Reservations[1].Id, Reservations[1].RoomId),
      new("A limpeza do quarto deixou a desejar", 3, Customers[2].Id, Reservations[2].Id, Reservations[2].RoomId),
      new("O café da manhã estava delicioso", 4, Customers[3].Id, Reservations[3].Id, Reservations[3].RoomId),
      new("Problemas com o Wi-Fi, muito lento", 2, Customers[4].Id, Reservations[4].Id, Reservations[4].RoomId),
    };

    for (var i = 0; i < 6; i++)
    {
      feedbacks[0].AddLike();
      feedbacks[0].AddDeslike();
      feedbacks[4].AddLike();
      feedbacks[2].AddDeslike();
    }

    await MockConnection.Context.Feedbacks.AddRangeAsync(feedbacks);
    await MockConnection.Context.SaveChangesAsync();

    Feedbacks = await MockConnection.Context.Feedbacks.ToListAsync();
  }

  public static async Task CreateEmployees()
  {
    await MockConnection.Context.Employees.AddRangeAsync([
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
    await MockConnection.Context.SaveChangesAsync();

    Employees = await MockConnection.Context.Employees.ToListAsync();
  }
  public static async Task CreateResponsabilities()
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
    
    await MockConnection.Context.Responsabilities.AddRangeAsync(responsabilities);
    await MockConnection.Context.SaveChangesAsync();

    Responsabilities = await MockConnection.Context.Responsabilities.ToListAsync();
  }

  public static async Task CreateRoomInvoices()
  {
    var roomInvoices = new List<RoomInvoice>()
    {
      Reservations[1].GenerateInvoice(EPaymentMethod.CreditCard,40),
      Reservations[2].GenerateInvoice(EPaymentMethod.Pix,10),
      Reservations[3].GenerateInvoice(EPaymentMethod.Pix,30),
      Reservations[4].GenerateInvoice(EPaymentMethod.CreditCard,16),
    };
    roomInvoices[0].FinishInvoice();

    await MockConnection.Context.RoomInvoices.AddRangeAsync(roomInvoices);
    await MockConnection.Context.SaveChangesAsync();

    RoomInvoices = await MockConnection.Context.RoomInvoices.ToListAsync();
  }

  public static async Task CreateCategories()
  {
    var categories = new List<Category>()
    {
      new("Quartos de luxo","Quartos de luxo com vista para a praia",120),
      new("Quartos médios", "Quartos de médio para uma hospedagem temporária", 40)
    };

    await MockConnection.Context.Categories.AddRangeAsync(categories);
    await MockConnection.Context.SaveChangesAsync();

    Categories = await MockConnection.Context.Categories.ToListAsync();
  }

  public static async Task CreateReports()
  {
    var reports = new List<Report>()
    {
      new("Vazamento de água no quarto 21","Vazamento de água no quarto 21 devido a encanação",EPriority.High,Employees[0],"Chamar o encanador"),
      new("Computador não liga", "Computador no escritório não está ligando, possivelmente problema na fonte de alimentação", EPriority.Critical, Employees[1], "Verificar a fonte de alimentação e os cabos"),
      new("Ar condicionado com defeito", "Ar condicionado da sala de reuniões está fazendo barulho estranho", EPriority.Medium, Employees[2], "Chamar o técnico de manutenção"),
      new("Falta de material de escritório", "Estoque de papel para impressão está baixo", EPriority.Low, Employees[3], "Requisitar mais papel para o almoxarifado"),
      new("Falha na rede Wi-Fi", "Rede Wi-Fi está instável no segundo andar", EPriority.High, Employees[4], "Verificar roteadores e pontos de acesso"),
      new("Lâmpada queimada", "Lâmpada do corredor principal está queimada", EPriority.Trivial, Employees[5], "Substituir a lâmpada queimada"),
      new("Cadeira quebrada", "Cadeira da sala de espera está quebrada", EPriority.Low, Employees[6], "Reparar ou substituir a cadeira"),
      new("Sistema de alarme disparado", "Sistema de alarme disparou sem motivo aparente", EPriority.Critical, Employees[7], "Chamar a equipe de segurança para verificar"),
      new("Telefone com problemas", "Telefone do setor de atendimento ao cliente não está funcionando", EPriority.Medium, Employees[8], "Verificar as conexões do telefone"),
      new("Janela com vazamento", "Janela da sala de conferências está com vazamento de água", EPriority.High, Employees[9], "Solicitar reparo na vedação da janela"),
      new("Problema na impressora", "Impressora do departamento financeiro está atolando papel", EPriority.Medium, Employees[10], "Realizar manutenção na impressora")
    };

    await MockConnection.Context.Reports.AddRangeAsync(reports);
    await MockConnection.Context.SaveChangesAsync();

    Reports = await MockConnection.Context.Reports.ToListAsync();
  }
  

  public static async Task CreateServices()
  {
    var services = new List<Service>
    {
      new("Limpeza", 30m, EPriority.Medium, 30),
      new("Manutenção", 40m, EPriority.High, 45),
      new("Café da Manhã", 20m, EPriority.Low, 60),
      new("Wi-Fi", 10m, EPriority.Trivial, 1),
      new("Serviço de Quarto", 50m, EPriority.Critical, 120)
    };

    await MockConnection.Context.Services.AddRangeAsync(services);
    await MockConnection.Context.SaveChangesAsync();

    Services = await MockConnection.Context.Services.ToListAsync();
  }

  public static async Task CreateReservations()
  {
    var reservations = new List<Reservation>()
    {
      new(Rooms[0], DateTime.Now, [Customers[0]],DateTime.Now.AddDays(2)),
      new(Rooms[1], DateTime.Now, [Customers[1]],DateTime.Now.AddDays(8)),
      new(Rooms[0], DateTime.Now, [Customers[2]],DateTime.Now.AddDays(5)),
      new(Rooms[1], DateTime.Now, [Customers[3]],DateTime.Now.AddDays(8)),
      new(Rooms[1], DateTime.Now, [Customers[4]],DateTime.Now.AddDays(5)),
    };

    reservations[0].AddService(Services[0]);
    reservations[1].AddService(Services[1]);
    reservations[2].AddService(Services[2]);
    reservations[3].AddService(Services[3]);
    reservations[4].AddService(Services[4]);

    await MockConnection.Context.Reservations.AddRangeAsync(reservations);
    await MockConnection.Context.SaveChangesAsync();

    Reservations = await MockConnection.Context.Reservations.ToListAsync();
  }


  public static async Task CreateRooms()
  {
    await MockConnection.Context.Rooms.AddRangeAsync([
      new(22, 50m, 3, "Um quarto para hospedagem.", Categories[0].Id),
      new(21, 40m, 4, "Um quarto com vista para a praia.", Categories[1].Id),
    ]);
    await MockConnection.Context.SaveChangesAsync();

    Rooms = await MockConnection.Context.Rooms.ToListAsync();
  }

}
