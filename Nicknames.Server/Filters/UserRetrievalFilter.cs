using Microsoft.AspNetCore.Mvc.Filters;
using Nicknames.Server.Services;
using Nicknames.Shared.Entities;
using System.Security.Claims;

namespace Nicknames.Server.Filters;

public class UserRetrievalFilter : IAsyncActionFilter
{
    private readonly IUserService _userService;

    public UserRetrievalFilter(IUserService userService)
    {
        _userService = userService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            string id = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            int platformInt;

            if (int.TryParse(context.HttpContext.User.FindFirst("aud").Value, out platformInt))
            {
                Platform platform = (Platform)platformInt;
                User user = await _userService.GetUser(platform, id);

                if (user != null)
                {
                    // Store the retrieved user in the HttpContext for use in controller actions.
                    context.HttpContext.Items["CurrentUser"] = user;
                }
            }
        }

        await next();
    }
}
