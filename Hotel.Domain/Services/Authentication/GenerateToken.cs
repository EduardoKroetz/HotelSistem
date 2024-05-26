using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Hotel.Domain.Services.Authentication;

public static partial class Authentication
{
  public static string GenerateToken(Admin admin)
  {
    var claims = new List<Claim>()
    {
      new(ClaimTypes.NameIdentifier, admin.Id.ToString()),
      new(ClaimTypes.Email, admin.Email.Address)
    };

    //Se for admin ou rootAdmin
    if (admin.IsRootAdmin) // Possui todo acesso
      claims.Add(new(ClaimTypes.Role, "RootAdmin"));
    else
      claims.Add(new(ClaimTypes.Role, "Admin"));

    //Adicionando as permissões no token
    var permissions = admin.Permissions.Select(x => x.Name).ToList(); //Pega todos os nomes das permissõs dos administradores
    claims.Add(new("permissions", string.Join(",",permissions))); //separa todas as permissões por vírgula

    //criar tokenDescriptor
    var key = Encoding.ASCII.GetBytes(Configuration.Configuration.JwtKey);
    var tokenDescriptor = new SecurityTokenDescriptor()
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddDays(2),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
    };

    //criar token
    return new JwtSecurityTokenHandler().CreateEncodedJwt(tokenDescriptor);
  }

  public static string GenerateToken(Customer customer)
  {
    var key = Encoding.ASCII.GetBytes(Configuration.Configuration.JwtKey);
    var tokenHandler = new JwtSecurityTokenHandler();
    var claims = new List<Claim>()
    {
      new(ClaimTypes.NameIdentifier, customer.Id.ToString()),
      new(ClaimTypes.Email, customer.Email.Address),
      new(ClaimTypes.Role, "Customer"),
    };
    var tokenDescriptor = new SecurityTokenDescriptor()
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddDays(2),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }

  public static string GenerateToken(Employee employee)
  {
    var key = Encoding.ASCII.GetBytes(Configuration.Configuration.JwtKey);
    var tokenHandler = new JwtSecurityTokenHandler();
    var claims = new List<Claim>()
    {
      new(ClaimTypes.NameIdentifier, employee.Id.ToString()),
      new(ClaimTypes.Email, employee.Email.Address),
      new(ClaimTypes.Role, "Employee"),
      new("Permissions", string.Join(",",employee.Permissions))
    };
    var tokenDescriptor = new SecurityTokenDescriptor()
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddDays(2),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
}
