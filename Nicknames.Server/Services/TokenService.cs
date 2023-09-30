using Microsoft.IdentityModel.Tokens;
using Nicknames.Server.Auth;
using Nicknames.Server.Storage;
using Nicknames.Shared.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Nicknames.Server.Services;

public interface ITokenService
{
    Task<string> UpdateUserToken(User user);

    Task<bool> IsTokenValid(User user, string token);

    Task<bool> IsTokenExpired(User user);
}

public class TokenService : ITokenService
{
    private readonly NicknamesDbContext _db;
    private readonly IUserService _userService;
    private readonly IConfiguration _config;

    public TokenService(NicknamesDbContext db, IConfiguration config, IUserService userService)
    {
        _db = db;
        _config = config;
        _userService = userService;
    }

    public Task<bool> IsTokenExpired(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var jwtToken = tokenHandler.ReadJwtToken(user.Token);

            if (jwtToken.ValidTo != null && jwtToken.ValidTo < DateTime.UtcNow)
            {
                return Task.FromResult(true);
            }
        }
        catch (Exception)
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    public Task<bool> IsTokenValid(User user, string token)
    {
        if(user.Token == token)
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    public async Task<string> UpdateUserToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["auth:private"])); // Replace with your secret key
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.GameId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: "NICKNAMES",
            audience: ((int)user.Platform).ToString(),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var completedToken = tokenHandler.WriteToken(token);

        user.Token = completedToken;
        _db.Users.Update(user);
        await _db.SaveChangesAsync();

        return completedToken;
    }
 
}
