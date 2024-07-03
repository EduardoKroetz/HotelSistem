
<h1 align="center">API de Sistema de Hotel</h1>

[MicrosoftSQLServer]: https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white
[.Net]: https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white
[C#]: https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white


![MicrosoftSQLServer]
![.Net]
![C#]

<p align="center">
   <a href="#getting-started">🚀 Começando</a> 
   <a href="#endpoints">🗺️ Rotas</a>
   <a href="#permissions">🔒 Permissões</a>
   <a href="#queries">🔎 Consultas </a>
</p>
<p align="center">
  <b>A API de sistema de hotel facilita a integração e permite realizar operações típicas de hospedagem. Ela permite realizar operações como gestão de reservas, cômodos, funcionários, clientes, relatórios e mais.</b>
</p>
<h2 id="getting-started">🚀 Começando</h2>
<h3>Pré-requisitos</h3>
<p>Os seguintes pré-requisitos são necessários para executar o projeto:</p>
<ul>
  <li><a href="https://dotnet.microsoft.com/pt-br/">.NET 8</a></li>
  <li><a href="https://github.com">Git 2</a></li>
</ul>
<h3 id="cloning">Cloning</h3>
<p>Para clonar o projeto, basta executar o seguinte comando no terminal:</p>

```bash
git clone https://github.com/EduardoKroetz/HotelSistem.git
```

<h3>Executar o Projeto com SQL Server Dockerizado</h3>
<p>Certifique-se de ter o Docker instalado em seu sistema. Caso ainda não tenha, você pode baixá-lo e instalá-lo a partir do <a href="https://www.docker.com/get-started" target="_blank">site oficial do Docker</a>. </p>

<h4>Baixando a Imagem do SQL Server:</h4>
Abra um terminal e execute o seguinte comando para baixar a imagem do SQL Server do Docker Hub:

```bash
docker pull mcr.microsoft.com/mssql/server:2022-latest
 ```

<h4>Executando o Container do SQL Server:</h4>
Para iniciar um container Docker com o SQL Server, utilize o seguinte comando, substituindo <sua_senha> pela senha desejada para o usuário SA_PASSWORD:

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=<sua_senha>" \
-p 1433:1433 --name sqlserver_instance \
-d mcr.microsoft.com/mssql/server:2022-latest
```

Na pasta "Hotel.Domain", atualize o banco de dados executando:

```bash
dotnet ef database update
```

<h3 id="environments">Variáveis de ambiente</h3>
<p>Adicione essas variáveis de ambiente em um arquivo <code>appsettings.json</code> na pasta Hotel.Domain:</p>

```json
{
  "EmailToSendEmail":"seu_email_para_enviar_emails",
  "PasswordToSendEmail":"sua_senha_para_enviar_emails",
  "JwtKey":"addakaDfAyrtcvnncvAEreaxxvrtkkadAeretGAc",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=HotelSystem;User ID=sa;Password=<sua_senha>;TrustServerCertificate=true"
  },
  "Stripe":  {
    "SecretKey":"sua_chave_secreta_do_stripe",
    "PublishableKey":"sua_chave_pública_do_stripe"
  }
}
```
<p>Observação: As chaves do Stripe são necessárias para a integração com o sistema de pagamento Stripe. Caso ainda não tenha uma conta no Stripe, você pode criar uma conta de teste <a href="https://docs.stripe.com/testing" target="_blank">aqui.</a></p>

<h3 id="start">Starting</h3>
<p>Para iniciar o projeto, execute os seguintes comandos:</p>

```bash
cd HotelSistem/Hotel.Domain
dotnet run
```

Por padrão, a aplicação será executada em http://localhost:5000/. Você pode acessar a interface gráfica da API em http://localhost:5000/swagger/index.html

<h2 id="endpoints">📍 API Endpoints</h2>

<h3>Admin</h3>

| Rota                                    | Descrição                                        |
|-----------------------------------------|--------------------------------------------------|
| <kbd>GET /v1/admins</kbd>               | Recupera uma lista de administradores            |
| <kbd>DELETE /v1/admins</kbd>            | Exclui o administrador autenticado               |
| <kbd>PUT /v1/admins</kbd>               | Atualiza os detalhes do administrador autenticado |
| <kbd>GET /v1/admins/{Id}</kbd>          | Recupera os detalhes de um administrador pelo ID |
| <kbd>PUT /v1/admins/{Id}</kbd>          | Atualiza os detalhes de um administrador pelo ID |
| <kbd>DELETE /v1/admins/{Id}</kbd>       | Exclui um administrador pelo ID                  |
| <kbd>POST /v1/admins/{adminId}/permissions/{permissionId}</kbd>  | Adiciona uma permissão a um administrador pelo ID |
| <kbd>DELETE /v1/admins/{adminId}/permissions/{permissionId}</kbd>| Remove uma permissão de um administrador pelo ID |
| <kbd>POST /v1/admins/to-root-admin/{toRootAdminId}</kbd>        | Promove um administrador a root pelo ID          |
| <kbd>PATCH /v1/admins/name</kbd>        | Atualiza o nome do administrador autenticado        |
| <kbd>PATCH /v1/admins/email</kbd>       | Atualiza o email do administrador autenticado       |
| <kbd>PATCH /v1/admins/phone</kbd>       | Atualiza o telefone do administrador autenticado    |
| <kbd>PATCH /v1/admins/address</kbd>     | Atualiza o endereço do administrador autenticado    |
| <kbd>PATCH /v1/admins/gender</kbd>      | Atualiza o gênero do administrador autenticado   |
| <kbd>PATCH /v1/admins/date-of-birth</kbd> | Atualiza a data de nascimento do administrador autenticado |

<h3>Customer</h3>

| Rota                                    | Descrição                                        |
|-----------------------------------------|--------------------------------------------------|
| <kbd>GET /v1/customers</kbd>            | Recupera uma lista de clientes                   |
| <kbd>PUT /v1/customers</kbd>            | Atualiza os detalhes do cliente autenticado      |
| <kbd>DELETE /v1/customers</kbd>         | Exclui o cliente autenticado                     |
| <kbd>GET /v1/customers/{Id}</kbd>       | Recupera os detalhes de um cliente pelo ID       |
| <kbd>PUT /v1/customers/{Id}</kbd>       | Atualiza os detalhes de um cliente pelo ID       |
| <kbd>DELETE /v1/customers/{Id}</kbd>    | Exclui um cliente pelo ID                        |
| <kbd>PATCH /v1/customers/name</kbd>     | Atualiza o nome do cliente autenticado           |
| <kbd>PATCH /v1/customers/email</kbd>    | Atualiza o email do cliente autenticado          |
| <kbd>PATCH /v1/customers/phone</kbd>    | Atualiza o telefone do cliente autenticado       |
| <kbd>PATCH /v1/customers/address</kbd>  | Atualiza o endereço do cliente autenticado       |
| <kbd>PATCH /v1/customers/gender</kbd>   | Atualiza o gênero de um cliente pelo ID         |
| <kbd>PATCH /v1/customers/date-of-birth</kbd> | Atualiza a data de nascimento do cliente autenticado |

<h3>Employee</h3> 

| Rota                                    | Descrição                                        |
|-----------------------------------------|--------------------------------------------------|
| <kbd>GET /v1/employees</kbd>            | Recupera uma lista de funcionários               |
| <kbd>DELETE /v1/employees</kbd>         | Exclui o funcionário autenticado                 |
| <kbd>PUT /v1/employees</kbd>            | Atualiza os detalhes do funcionário autenticado  |
| <kbd>GET /v1/employees/{id}</kbd>       | Recupera os detalhes de um funcionário pelo ID   |
| <kbd>PUT /v1/employees/{id}</kbd>       | Atualiza os detalhes de um funcionário pelo ID   |
| <kbd>DELETE /v1/employees/{id}</kbd>    | Exclui um funcionário pelo ID                    |
| <kbd>POST /v1/employees/{id}/responsibilities/{resId}</kbd>  | Adiciona uma responsabilidade a um funcionário pelo ID |
| <kbd>DELETE /v1/employees/{id}/responsibilities/{resId}</kbd>| Remove uma responsabilidade de um funcionário pelo ID |
| <kbd>POST /v1/employees/{employeeId}/permissions/{permissionId}</kbd> | Adiciona uma permissão a um funcionário pelo ID |
| <kbd>DELETE /v1/employees/{employeeId}/permissions/{permissionId}</kbd> | Remove uma permissão de um funcionário pelo ID |
| <kbd>PATCH /v1/employees/name</kbd>     | Atualiza o nome do funcionário autenticado       |
| <kbd>PATCH /v1/employees/email</kbd>    | Atualiza o email do funcionário autenticado      |
| <kbd>PATCH /v1/employees/phone</kbd>    | Atualiza o telefone do funcionário autenticado   |
| <kbd>PATCH /v1/employees/address</kbd>  | Atualiza o endereço do funcionário autenticado   |
| <kbd>PATCH /v1/employees/gender</kbd>   | Atualiza o gênero de um funcionário pelo ID     |
| <kbd>PATCH /v1/employees/date-of-birth</kbd> | Atualiza a data de nascimento do funcionário autenticado |

<h3>Feedback</h3> 

| Rota                                    | Descrição                                        |
|-----------------------------------------|--------------------------------------------------|
| <kbd>GET /v1/feedbacks</kbd>            | Recupera uma lista de feedbacks                  |
| <kbd>POST /v1/feedbacks</kbd>           | Cria um novo feedback                            |
| <kbd>GET /v1/feedbacks/{Id}</kbd>       | Recupera os detalhes de um feedback pelo ID      |
| <kbd>PUT /v1/feedbacks/{Id}</kbd>       | Atualiza os detalhes de um feedback pelo ID      |
| <kbd>DELETE /v1/feedbacks/{Id}</kbd>    | Exclui um feedback pelo ID                       |
| <kbd>PATCH /v1/feedbacks/{Id}/rate/{rate}</kbd> | Atualiza a avaliação de um feedback pelo ID  |
| <kbd>PATCH /v1/feedbacks/{Id}/comment</kbd> | Atualiza o comentário de um feedback pelo ID  |
| <kbd>PATCH /v1/feedbacks/add-like/{feedbackId}</kbd> | Adiciona um like a um feedback pelo ID |
| <kbd>PATCH /v1/feedbacks/remove-like/{feedbackId}</kbd> | Remove um like de um feedback pelo ID |
| <kbd>PATCH /v1/feedbacks/add-dislike/{feedbackId}</kbd> | Adiciona um dislike a um feedback pelo ID |
| <kbd>PATCH /v1/feedbacks/remove-dislike/{feedbackId}</kbd> | Remove um dislike de um feedback pelo ID |

<h3>Invoice</h3> 

| Rota                                    | Descrição                                        |
|-----------------------------------------|--------------------------------------------------|
| <kbd>GET /v1/invoices</kbd>        | Recupera uma lista de faturas                    |
| <kbd>GET /v1/invoices/my</kbd>     | Recupera as faturas do usuário autenticado       |
| <kbd>GET /v1/invoices/{Id}</kbd>   | Recupera os detalhes de uma fatura  pelo ID      |
| <kbd>DELETE /v1/invoices/{Id}</kbd>| Exclui uma fatura pelo ID                        |

<h3>Login</h3>

| Rota                                    | Descrição                                        |
|-----------------------------------------|--------------------------------------------------|
| <kbd>POST /v1/login</kbd>               | Realiza o login de qualquer tipo de usuário      |

<h3>Permission</h3>

| Rota                                    | Descrição                                        |
|-----------------------------------------|--------------------------------------------------|
| <kbd>GET /v1/permissions</kbd>          | Recupera uma lista de permissões                 |
| <kbd>GET /v1/permissions/{Id}</kbd>     | Recupera os detalhes de uma permissão pelo ID    |

<h3>Register</h3>

| Rota                                    | Descrição                                        |
|-----------------------------------------|--------------------------------------------------|
| <kbd>POST /v1/register/customers</kbd>  | Registra um novo cliente                         |
| <kbd>POST /v1/register/admins</kbd>     | Registra um novo administrador                   |
| <kbd>POST /v1/register/employees</kbd>  | Registra um novo funcionário                     |

<h3>Report</h3>

| Rota                                    | Descrição                                        |
|-----------------------------------------|--------------------------------------------------|
| <kbd>GET /v1/reports</kbd>              | Recupera uma lista de relatórios                 |
| <kbd>POST /v1/reports</kbd>             | Cria um novo relatório                           |
| <kbd>GET /v1/reports/{Id}</kbd>         | Recupera os detalhes de um relatório pelo ID     |
| <kbd>PUT /v1/reports/{Id}</kbd>         | Atualiza os detalhes de um relatório pelo ID     |
| <kbd>DELETE /v1/reports/my/{Id}</kbd>   | Exclui um relatório do usuário atual pelo ID     |
| <kbd>PATCH /v1/reports/finish/{Id}</kbd>| Finaliza um relatório pelo ID                    |
| <kbd>PATCH /v1/reports/cancel/{Id}</kbd>| Cancela um relatório pelo ID                     |
| <kbd>PATCH /v1/reports/priority/{id}</kbd> | Atualiza a prioridade de um relatório pelo ID |

<h3>Reservation</h3>

| Rota                                    | Descrição                                        |
|-----------------------------------------|--------------------------------------------------|
| <kbd>GET /v1/reservations</kbd>         | Recupera uma lista de reservas                   |
| <kbd>GET /v1/reservations/{Id}</kbd>    | Recupera os detalhes de uma reserva pelo ID      |
| <kbd>POST /v1/reservations</kbd>        | Cria uma nova reserva                            |
| <kbd>DELETE /v1/reservations/{Id}</kbd> | Exclui uma reserva pelo ID                       |
| <kbd>PATCH /v1/reservations/expected-check-out/{Id}</kbd> | Atualiza o check-out esperado de uma reserva pelo ID |
| <kbd>PATCH /v1/reservations/expected-check-in/{Id}</kbd>  | Atualiza o check-in esperado de uma reserva pelo ID  |
| <kbd>POST /v1/reservations/{Id}/services/{serviceId}</kbd> | Adiciona um serviço a uma reserva pelo ID        |
| <kbd>DELETE /v1/reservations/{Id}/services/{serviceId}</kbd>| Remove um serviço de uma reserva pelo ID        |
| <kbd>POST /v1/reservations/check-in/{Id}</kbd>  | Faz o check-in de uma reserva pelo ID            |
| <kbd>POST /v1/reservations/finish/{Id}</kbd>    | Finaliza uma reserva pelo ID                     |
| <kbd>POST /v1/reservations/cancel/{Id}</kbd>    | Cancela uma reserva pelo ID                      |
| <kbd>GET /v1/reservations/total-amount</kbd>    | Simula o preço total de uma reserva           |


<h3>Responsibility</h3>

| Rota                                    | Descrição                                        |
|-----------------------------------------|--------------------------------------------------|
| <kbd>GET /v1/responsibilities</kbd>     | Recupera uma lista de responsabilidades          |
| <kbd>POST /v1/responsibilities</kbd>    | Cria uma nova responsabilidade                   |
| <kbd>GET /v1/responsibilities/{Id}</kbd>| Recupera os detalhes de uma responsabilidade pelo ID |
| <kbd>PUT /v1/responsibilities/{Id}</kbd>| Atualiza os detalhes de uma responsabilidade pelo ID |
| <kbd>DELETE /v1/responsibilities/{Id}</kbd>| Exclui uma responsabilidade pelo ID             |

<h3>Room</h3>

| Rota                                    | Descrição                                        |
|-----------------------------------------|--------------------------------------------------|
| <kbd>GET /v1/rooms</kbd>                | Recupera uma lista de quartos                    |
| <kbd>GET /v1/rooms/{Id}</kbd>           | Recupera os detalhes de um quarto pelo ID        |
| <kbd>PUT /v1/rooms/{Id}</kbd>           | Atualiza os detalhes de um quarto pelo ID        |
| <kbd>POST /v1/rooms</kbd>               | Cria um novo quarto                              |
| <kbd>DELETE /v1/rooms/{Id}</kbd>        | Exclui um quarto pelo ID                         |
| <kbd>POST /v1/rooms/{Id}/services/{serviceId}</kbd> | Adiciona um serviço a um quarto pelo ID   |
| <kbd>DELETE /v1/rooms/{Id}/services/{serviceId}</kbd> | Remove um serviço de um quarto pelo ID  |
| <kbd>PATCH /v1/rooms/number/{Id}</kbd>   | Atualiza o número de um quarto pelo ID           |
| <kbd>PATCH /v1/rooms/name/{Id}</kbd>     | Atualiza o nome de um quarto pelo ID             |
| <kbd>PATCH /v1/rooms/capacity/{Id}</kbd> | Atualiza a capacidade de um quarto pelo ID       |
| <kbd>PATCH /v1/rooms/category/{Id}</kbd> | Atualiza a categoria de um quarto pelo ID        |
| <kbd>PATCH /v1/rooms/price/{Id}</kbd>    | Atualiza o preço de um quarto pelo ID            |
| <kbd>PATCH /v1/rooms/enable/{Id}</kbd>   | Ativa um quarto pelo ID                          |
| <kbd>PATCH /v1/rooms/disable/{Id}</kbd>  | Desativa um quarto pelo ID                       |
| <kbd>PATCH /v1/rooms/available/{Id}</kbd>| Atualiza o status de 'fora de serviço' para 'disponível' de um quarto pelo ID |

<h3>Service</h3>

| Rota                                    | Descrição                                        |
|-----------------------------------------|--------------------------------------------------|
| <kbd>GET /v1/services</kbd>             | Recupera uma lista de serviços                   |
| <kbd>POST /v1/services</kbd>            | Cria um novo serviço                             |
| <kbd>GET /v1/services/{Id}</kbd>        | Recupera os detalhes de um serviço pelo ID       |
| <kbd>PUT /v1/services/{Id}</kbd>        | Atualiza os detalhes de um serviço pelo ID       |
| <kbd>DELETE /v1/services/{Id}</kbd>     | Exclui um serviço pelo ID                        |
| <kbd>POST /v1/services/{Id}/responsibilities/{responsibilityId}</kbd> | Adiciona uma responsabilidade a um serviço pelo ID |
| <kbd>DELETE /v1/services/{Id}/responsibilities/{responsibilityId}</kbd>| Remove uma responsabilidade de um serviço pelo ID |

<h3>Verification</h3>

| Rota                                    | Descrição                                        |
|-----------------------------------------|--------------------------------------------------|
| <kbd>POST /v1/verifications/email-code</kbd> | Envia um código de verificação de email por emai        |


<h2 id="permissions">🔒 Permissões</h2>
<p>A API permite que funcionários e administradores (por padrão) gerenciem permissões tanto de administradores quanto de funcionários, bloqueando ou liberando acesso a diversas funcionalidades. Isso pode ser feito através dos seguintes endpoints:</p>

<h3>Administradores</h3>

| Rota                                    | Descrição                                        |
|-----------------------------------------|--------------------------------------------------|
| <kbd>POST /v1/admins/{adminId}/permissions/{permissionId}</kbd>  | Adiciona uma permissão a um administrador pelo ID |
| <kbd>DELETE /v1/admins/{adminId}/permissions/{permissionId}</kbd>| Remove uma permissão de um administrador pelo ID |

<h3>Funcionários</h3>

| Rota                                    | Descrição                                        |
|-----------------------------------------|--------------------------------------------------|
| <kbd>POST /v1/employees/{employeeId}/permissions/{permissionId}</kbd> | Adiciona uma permissão a um funcionário pelo ID |
| <kbd>DELETE /v1/employees/{employeeId}/permissions/{permissionId}</kbd> | Remove uma permissão de um funcionário pelo ID |

<h2 id="queries">🔎 Consultas</h2>
<p>É possível fazer filtragem de dados em todas as tabelas do banco de dados através do método GET em endpoints como "v1/admins", "v1/reservations", "v1/rooms" somente incluindo o campo que você quer filtrar na Query. Exemplo:</p>

| Rota                                    | Retorno                                     |
|-----------------------------------------|--------------------------------------------------|
| <kbd>POST /v1/reservations?skip=0&take=25&roomId=739a6c2e-5957-467e-808a-c508f49629a8</kbd> | Busca todas as reservas associadas a um determinado cômodo |
| <kbd>POST /v1/rooms?skip=0&take=5&categoryId=739a6c2e-5957-467e-808a-c508f49629a8</kbd> | Busca todos os cômodos associados a uma determinada categoria |

<p>Para campos númericos ou de data, é necessário incluir um operador para filtrar os dados. Os seguintes operadores disponíveis são:
<p>"lt" = menor que</p>
<p>"eq" = igual que</p>
<p>"gt" = maior que</p>
Isso permite que você filtre dados conforme precisar de uma maneira flexível.
Exemplo:</p>

| Rota                                    | Retorno                                     |
|-----------------------------------------|--------------------------------------------------|
| <kbd>POST /v1/reservations?skip=0&take=25&createdAt=2024-07-03T00:00:00&createdAtOperator=gt</kbd> | Busca todas as reservas com a criação maior que o dia 03/07/2024|

<h3>Campos disponíveis para consulta</h3>

| Rota                                    | Campos disponíveis                               |
|-----------------------------------------|--------------------------------------------------|
| <kbd>GET /v1/admins</kbd>|skip, take, name, email, phone, gender, dateOfBirth, dateOfBirthOperator, createdAt, createdAtOperator, isRootAdmin, permissionId|
| <kbd>GET /v1/categories</kbd>| skip, take, name, averagePrice, averagePriceOperator, roomId|
| <kbd>GET /v1/customers </kbd>| skip, take, name, email, phone, gender, dateOfBirth, dateOfBirthOperator, createdAt, createdAtOperator|
| <kbd>GET /v1/employees </kbd>| skip, take, name, email, phone, gender, dateOfBirth, dateOfBirthOperator, createdAt, createdAtOperator, salary, salaryOperator|
| <kbd>GET /v1/feedbacks </kbd>| skip, take, createdAt, createdAtOperator, comment, rate, rateOperator, likes, likesOperator, dislikes, dislikesOperator, updatedAt, updatedAtOperator, customerId, reservationId, roomId|
| <kbd>GET /v1/invoices </kbd>| skip, take, paymentMethod, totalAmount, totalAmountOperator, customerId, reservationId, serviceId|
| <kbd>GET /v1/permissions </kbd>| skip, take, createdAt, createdAtOperator, name, isActive, adminId|
| <kbd>GET /v1/reports </kbd>| skip, take, summary, status, priority, employeeId, createdAt, createdAtOperator|
| <kbd>GET /v1/reservations </kbd>| skip, take, timeHosted, timeHostedOperator, dailyRate, dailyRateOperator, checkIn, checkInOperator, checkOut, checkOutOperator, status, capacity, capacityOperator, roomId, customerId, invoiceId, serviceId, createdAt, createdAtOperator, expectedCheckIn, expectedCheckInOperator, expectedCheckOut, expectedCheckOutOperator, expectedTimeHosted, expectedTimeHostedOperator|
| <kbd>GET /v1/responsibilities </kbd>| skip, take, name, priority, employeeId, serviceId, createdAt, createdAtOperator| 
| <kbd>GET /v1/rooms </kbd>| skip, take, name, number, numberOperator, price, priceOperator, status, capacity, capacityOperator, serviceId, categoryId, createdAt, createdAtOperator|
| <kbd>GET /v1/services </kbd>| skip, take, name, price, priceOperator, priority, isActive, timeInMinutes, timeInMinutesOperator, responsibilityId, reservationId, invoiceId, roomId, createdAt, createdAtOperator|


