
<h1> API de Sistema de hotel</h1>

![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)

<p align="center">
   <a href="#started">Getting Started</a> • 
</p>

<p align="center">
  <b>A API de sistema de hotel facilita a integração e permite realizar operações típicas de hospedagem. Ela permite realizar operações como gestão de reservas, cômodos, funcionários, clientes, relatórios e mais.</b>
</p>

<h2 id="started">🚀 Getting started</h2>
<h3>Prerequisites</h3>
<p>Os seguintes pré-requisitos são necessários para executar o projeto</p>

- [.NET 8](https://dotnet.microsoft.com/pt-br/)
- [Git 2](https://github.com)

<h3 id="cloning">Cloning</h3>
<p>Para clonar o projeto, basta executar o seguinte comando no terminal</p>

```bash
git clone https://github.com/EduardoKroetz/HotelSistem.git
```

<h3 id="environments"> Environment Variables</h2>
<p>Adicione essas variáveis de ambiente em um arquivo appsettings.json</p>

```yaml
EmailToSendEmail={seu_email_para_enviar_emails}
PasswordToSendEmail={sua_senha_para_enviar_emails}
JwtKey=addakaDfAyrtcvnncvAEreaxxvrtkkadAeretGAc
ConnectionStrings:DefaultConnection={sua_conexão_com_o_sql_server}
Stripe:SecretKey={sua_chave_secreta_do_stripe}
Stripe:PublishableKey={sua_chave_pública_do_stripe}
```

<h3 id="start">Starting</h3>
<p>Para iniciar o projeto, execute</p>

```bash
cd HotelSistem/Hotel.Domain
dotnet run
```
