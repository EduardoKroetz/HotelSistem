﻿using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.EmployeeEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Services.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Hotel.Domain.Services.TokenServices;

public class TokenService
{
    public string GenerateToken(Admin admin)
    {
        var claims = new List<Claim>()
        {
          new(ClaimTypes.NameIdentifier, admin.Id.ToString()),
          new(ClaimTypes.Email, admin.Email.Address)
        };

        //Se for admin ou rootAdmin
        if (admin.IsRootAdmin) // Possui todo acesso
            claims.Add(new(ClaimTypes.Role, nameof(ERoles.RootAdmin)));
        else
            claims.Add(new(ClaimTypes.Role, nameof(ERoles.Admin)));

        var permissions = admin.Permissions.Select(x => (int)AuthorizationService.ConvertToPermission(x.Name)).ToList(); //Pega todos os enumeradores das permissõs dos administradores
        claims.Add(new("permissions", string.Join(",", permissions))); //separa todas as permissões por vírgula

        //criar tokenDescriptor
        var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        //criar token
        return new JwtSecurityTokenHandler().CreateEncodedJwt(tokenDescriptor);
    }

    public string GenerateToken(Customer customer)
    {
        var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, customer.Id.ToString()),
            new(ClaimTypes.Email, customer.Email.Address),
            new(ClaimTypes.Role, nameof(ERoles.Customer)),
        };
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        return new JwtSecurityTokenHandler().CreateEncodedJwt(tokenDescriptor);

    }

    public string GenerateToken(Employee employee)
    {
        var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new(ClaimTypes.Email, employee.Email.Address),
            new(ClaimTypes.Role, nameof(ERoles.Employee))
        };

        var permissions = employee.Permissions.Select(x => (int)(EPermissions)Enum.Parse(typeof(EPermissions), x.Name)).ToList(); //Pega todos os enumeradores das permissõs dos funcionários
        claims.Add(new("permissions", string.Join(",", permissions))); //separa todas as permissões por vírgula

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        return new JwtSecurityTokenHandler().CreateEncodedJwt(tokenDescriptor);
    }
}
