using Microsoft.Extensions.Configuration;
using Nicknames.Shared.Entities;

namespace Nicknames.Server.Auth;

public interface OIDHandler
{
    public Task<AuthResult> CheckProof(IConfiguration config, string id, string proof);
}
