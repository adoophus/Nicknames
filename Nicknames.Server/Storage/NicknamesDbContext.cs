using Microsoft.EntityFrameworkCore;
using Nicknames.Shared.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Nicknames.Server.Storage;

public class NicknamesDbContext : DbContext
{
    public NicknamesDbContext() { }

    public NicknamesDbContext(DbContextOptions<NicknamesDbContext> options)
    : base(options)
    {
    }

    public DbSet<User>? Users { get; set; } = default;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
