using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Horizon.Server.Modules.Users.Domain.Entities;
using Horizon.Server.Modules.Users.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Horizon.Server.Modules.Users.Infrastructure.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("userId", user.Id.ToString())
        };

        if (user.Role != null)
        {
            claims.Add(new Claim(ClaimTypes.Role, user.Role.Code));

            switch (user.Role.Code)
            {
                case "Admin":
                    claims.Add(new Claim("permission", "request:view-all"));
                    claims.Add(new Claim("permission", "request:approve-hr"));
                    claims.Add(new Claim("permission", "balance:adjust"));
                    claims.Add(new Claim("permission", "user:manage"));
                    claims.Add(new Claim("permission", "role:manage"));
                    break;

                case "HRManager":
                    claims.Add(new Claim("permission", "request:view-all"));
                    claims.Add(new Claim("permission", "request:approve-hr"));
                    claims.Add(new Claim("permission", "balance:adjust"));
                    claims.Add(new Claim("permission", "balance:view-all"));
                    claims.Add(new Claim("permission", "calendar:manage"));
                    break;

                case "DepartmentManager":
                    claims.Add(new Claim("permission", "request:view-subordinate"));
                    claims.Add(new Claim("permission", "request:approve-subordinate"));
                    claims.Add(new Claim("permission", "leave:view-subordinate"));
                    claims.Add(new Claim("permission", "request:create"));
                    claims.Add(new Claim("permission", "request:view-own"));
                    claims.Add(new Claim("permission", "leave:view-own"));
                    break;

                case "Employee":
                    claims.Add(new Claim("permission", "request:create"));
                    claims.Add(new Claim("permission", "request:view-own"));
                    claims.Add(new Claim("permission", "request:edit-own"));
                    claims.Add(new Claim("permission", "leave:view-own"));
                    claims.Add(new Claim("permission", "calendar:view-own"));
                    break;
            }
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = _jwtSettings.Audience,
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
            ValidateLifetime = false  // токен мог истечь
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            // Проверяем, что это действительно JWT токен
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }
}