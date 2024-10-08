﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GamesLand.Core.Users.Entities;
using GamesLand.Core.Users.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GamesLand.Infrastructure.PostgreSQL.Users;

public class UserAuthentication : IUserAuthentication
{
    private readonly IConfiguration _configuration;

    public UserAuthentication(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Match(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    public string GetToken(User user)
    {
        var claims = new[] { new Claim(ClaimTypes.Email, user.Email) };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:secret"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(null, null, claims,
            expires: DateTime.Now.AddYears(10), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public string GetUserEmailFromToken(string jwt)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var token = jwtHandler.ReadJwtToken(jwt);
        return token.Claims.First(c => c.Type == ClaimTypes.Email).Value;
    }
}