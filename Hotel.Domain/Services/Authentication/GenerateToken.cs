using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Hotel.Domain.Services.Authentication;

public partial class Authentication
{
  public string GenerateToken(Admin admin)
  {
    var key = Encoding.ASCII.GetBytes(Configuration.Configuration.JwtKey);
    var tokenHandler = new JwtSecurityTokenHandler();
    var claims = new List<Claim>()
    {
      new(ClaimTypes.NameIdentifier, admin.Id.ToString()),
      new(ClaimTypes.Email, admin.Email.Address),
      new(ClaimTypes.Role, "Admin"),
      new("Permissions", string.Join(",",admin.Permissions))
    };
    var tokenDescriptor = new SecurityTokenDescriptor()
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddDays(2),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }

  public string GenerateToken(Customer customer)
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

  public string GenerateToken(Employee employee)
  {
    var key = Encoding.ASCII.GetBytes(Configuration.Configuration.JwtKey);
    var tokenHandler = new JwtSecurityTokenHandler();
    var claims = new List<Claim>()
    {
      new(ClaimTypes.NameIdentifier, employee.Id.ToString()),
      new(ClaimTypes.Email, employee.Email.Address),
      new(ClaimTypes.Role, "Employee"),
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
