using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Nicknames.Server.Services;
using Nicknames.Shared.Entities;

namespace Nicknames.Server.Controllers;

public class AuthenticationController : Controller
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AuthenticationController(IAuthService authService, IUserService userService, ITokenService tokenService)
    {
        _authService = authService;
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpGet("~/signin")]
    public async Task<IActionResult> SignIn(string name, string provider, string id, string proof, string token)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(provider) 
            || (string.IsNullOrWhiteSpace(proof)) && string.IsNullOrWhiteSpace(token))
        {
            return BadRequest();
        }

        Platform? platform = await GetPlatformFromString(provider);

        if(platform == null)
        {
            return BadRequest();
        }

        User user = await _userService.GetUser((Platform)platform, id);

        // Baseline deny the request from authorising.
        AuthResult authResult = new AuthResult((Platform)platform, false);
        bool isTokenAuthed = false;
        bool tokenExpired = true;

        // We can authenticate the user with the JWT they give us instead.
        if (user != null && !string.IsNullOrWhiteSpace(token))
        {
            tokenExpired = await _tokenService.IsTokenExpired(user);

            // Check to see if the token they're presenting matches what they have in
            // the database and also check to see if it has expired.
            if (await _tokenService.IsTokenValid(user, token) && tokenExpired == false)
            {
                isTokenAuthed = true;
                authResult = new AuthResult((Platform)platform, true);
            }
            else
            {
                return Unauthorized( new { response = AuthResponse.TokenExpired });
            }
        }
        else if(user == null && !string.IsNullOrWhiteSpace(token))
        {
            return Unauthorized(new { response = AuthResponse.TokenExpired });
        }

        // No need to run any OpenID calls if we're able to verify
        // that the user's JWT is correct.
        if (!isTokenAuthed)
            authResult = await _authService.AuthUser((Platform)platform, id, proof);

        // If no attempts to auth have passed, fail the user.
        if (!authResult.HasPassed())
        {
            return Unauthorized(new { response = AuthResponse.AllAttemptsFailed });
        }

        if (user == null)
        {
            user = await _userService.CreateUser(new User() { 
                GameId = id,
                Nickname = name,
                Platform = authResult.GetPlatform(),
            });
        }

        // Generate a new user token for the user if
        // they need it.
        if (tokenExpired)
        {
            string userToken = await _tokenService.UpdateUserToken(user);

            return Ok(new { token = userToken });
        }

        return Ok(); // Successful authentication
    }

    [HttpGet("~/signout")]
    public IActionResult SignOutCurrentUser()
    {
        return SignOut(new AuthenticationProperties { RedirectUri = "/" },
            CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public async Task<Platform?> GetPlatformFromString(string platform)
    {
        platform = platform.ToLower();

        switch (platform)
        {
            case "steam":
                return Platform.Steam;
            case "microsoft":
                return Platform.Microsoft;
            case "epicgames":
                return Platform.EpicGames;
        }

        return null;
    }
}
