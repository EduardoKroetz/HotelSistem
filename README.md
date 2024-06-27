
<h1 align="center">API de Sistema de Hotel</h1>

[MicrosoftSQLServer]: https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white
[.Net]: https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white
[C#]: https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white


![MicrosoftSQLServer]
![.Net]
![C#]

<p align="center">
   <a href="#getting-started">🚀 Getting Started</a> • 
</p>
<p align="center">
  <b>A API de sistema de hotel facilita a integração e permite realizar operações típicas de hospedagem. Ela permite realizar operações como gestão de reservas, cômodos, funcionários, clientes, relatórios e mais.</b>
</p>
<h2 id="getting-started">🚀 Getting Started</h2>
<h3>Prerequisites</h3>
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

<h3 id="environments">Environment Variables</h3>
<p>Adicione essas variáveis de ambiente em um arquivo <code>appsettings.json</code>:</p>

```json
   "EmailToSendEmail":"seu_email_para_enviar_emails"
   "PasswordToSendEmail":"sua_senha_para_enviar_emails"
   "JwtKey":"addakaDfAyrtcvnncvAEreaxxvrtkkadAeretGAc"
   "ConnectionStrings": {
      "DefaultConnection":"sua_conexão_com_o_sql_server"
   }
   "Stripe":  {
      "SecretKey":"sua_chave_secreta_do_stripe"
      "PublishableKey":"sua_chave_pública_do_stripe"
   }
```
<p>Observação: As chaves do Stripe são necessárias para a integração com o sistema de pagamento Stripe. Caso ainda não tenha uma conta no Stripe, você pode criar uma conta de teste <a href="https://docs.stripe.com/testing">aqui.</a></p>

<h3 id="start">Starting</h3>
<p>Para iniciar o projeto, execute os seguintes comandos:</p>

```bash
cd HotelSistem/Hotel.Domain
dotnet run
```
