using Nicknames.Shared.Entities;

namespace Nicknames.Server.Auth;

public class EpicAuth : OIDHandler
{
    public Task<AuthResult> CheckProof(IConfiguration config, string id, string proof)
    {
        throw new NotImplementedException();
    }
}
