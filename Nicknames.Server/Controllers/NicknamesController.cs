using AngleSharp.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nicknames.Server.Exceptions;
using Nicknames.Server.Services;
using Nicknames.Server.Storage;
using Nicknames.Shared.Entities;
using System.Security.Claims;

namespace Nicknames.Server.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize("BearerPolicy")]
public class NicknamesController : ControllerBase
{
    private readonly IUserService _userService;

    public NicknamesController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("GetNickname")]
    public async Task<IActionResult> GetNicknameAsync()
    {
        if (HttpContext.Items.TryGetValue("CurrentUser", out var userObj) && userObj is User user)
        {
            string? nickname = user.Nickname;

            if (nickname == null)
            {
                throw new NotFoundException("Nickname not found");
            }

            return Ok(nickname);
        }

        throw new NotFoundException("User not found");
    }

    [HttpPost("SetNickname")]
    public async Task<IActionResult> SetNicknameAsync(int id, [FromBody] string nickname)
    {
        if (HttpContext.Items.TryGetValue("CurrentUser", out var userObj) && userObj is User user)
        {
            string? existingNickname = user.Nickname;

            if (existingNickname == null)
            {
                throw new NotFoundException("Nickname not found");
            }

            user.Nickname = nickname;
            await _userService.UpdateUserAsync(user);

            return Ok(nickname);
        }

        throw new NotFoundException("User not found");
    }
}