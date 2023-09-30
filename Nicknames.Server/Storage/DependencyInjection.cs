using Microsoft.EntityFrameworkCore;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System;

namespace Nicknames.Server.Storage;

public static class DependencyInjection
{
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("NicknamesDB") ?? "server=localhost;database=nicknames;user=root;password=";

        services.AddDbContextPool<NicknamesDbContext>(options => options.UseMySQL(connectionString));

        return services;
    }
}
