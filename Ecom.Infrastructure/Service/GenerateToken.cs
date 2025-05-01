using Ecom.Core.Entities;
using Ecom.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecom.Infrastructure.Service;

public class GenerateToken : IGenerateToken
{
    private readonly IConfiguration _configuration;

    public GenerateToken(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public  string GetAndCreateToken(AppUser user)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name,user.UserName),
            new Claim(ClaimTypes.Email,user.Email)
        };                               
        var key = _configuration["Jwt:Key"];
        var KeyEconding=Encoding.ASCII.GetBytes(key);
        SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(KeyEconding), SecurityAlgorithms.HmacSha256);
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject= new ClaimsIdentity(claims),
            Expires=DateTime.Now.AddDays(1),
            Issuer= _configuration["Jwt:Isuuer"],
            SigningCredentials= signingCredentials,
            NotBefore= DateTime.Now,
        };
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        
        var token= handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
}