using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Nicknames.Server.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<ITokenService, TokenService>();

        return services;
    }
}
