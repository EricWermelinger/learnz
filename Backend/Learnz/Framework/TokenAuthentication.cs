﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Learnz.Framework;

public static class TokenAuthentication
{
    public static TokenDTO CreateToken(Guid userId, IConfiguration configuration)
    {
        string jwtSecret = configuration["JWT:Token"];
        int lifetime = Int32.Parse(configuration["JWT:LifeTime"]);
        int refreshLifetime = Int32.Parse(configuration["JWT:RefreshLifeTime"]);

        List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userId.ToString())
            };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecret));

        var credits = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddMinutes(lifetime),
            signingCredentials: credits);

        string refreshToken = GenerateRefreshToken();

        return new TokenDTO
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            RefreshExpires = DateTime.UtcNow.AddMinutes(refreshLifetime)
        };
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
