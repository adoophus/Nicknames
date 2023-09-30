using Microsoft.EntityFrameworkCore;
using Nicknames.Server.Storage;
using Nicknames.Shared.Entities;
using System;

#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8604

namespace Nicknames.Server.Services;

public interface IUserService
{
    Task<User> CreateUser(User user);
    Task<User> GetUser(Platform platform, string id);
    Task UpdateUserAsync(User user);
}

public class UserService : IUserService
{
    private readonly NicknamesDbContext _db;

    public UserService(NicknamesDbContext db)
    {
       _db = db;
    }

    public async Task<User> CreateUser(User user)
    {
        await _db.AddAsync(user);
        await _db.SaveChangesAsync();

        return user;
    }

    public async Task<User> GetUser(Platform platform, string id)
    {
        return _db.Users.Where(x => x.Platform == platform)
            .Where(x => x.GameId == id)
            .FirstOrDefault();
    }

    public async Task UpdateUserAsync(User user)
    {
        _db.Entry(user).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }
}
