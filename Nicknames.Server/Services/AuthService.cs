using Nicknames.Server.Auth;
using Nicknames.Server.Storage;
using Nicknames.Shared.Entities;

namespace Nicknames.Server.Services;

public interface IAuthService
{
    Task<AuthResult> AuthUser(Platform platform, string userId, string proof);

    public static Platform GetPlatformFromString(string platform) { return Platform.Steam; }
}

public class AuthService : IAuthService
{
    private readonly NicknamesDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(NicknamesDbContext db, IConfiguration configuration)
    {
        _db = db;
        _config = configuration;
    }

    public async Task<AuthResult> AuthUser(Platform platform, string userId, string proof)
    {
        switch(platform)
        {
            case Platform.Steam:
                return await new SteamAuth().CheckProof(_config, userId, proof);
            case Platform.Microsoft:
                return await new MicrosoftAuth().CheckProof(_config, userId, proof);
            case Platform.EpicGames:
                return await new EpicAuth().CheckProof(_config, userId, proof);
        }

        return new AuthResult(false);
    }

}
