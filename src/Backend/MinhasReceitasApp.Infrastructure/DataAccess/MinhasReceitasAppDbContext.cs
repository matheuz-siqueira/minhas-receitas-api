using Microsoft.EntityFrameworkCore;
using MinhasReceitasApp.Domain.Entities;

namespace MinhasReceitasApp.Infrastructure.DataAccess;

public class MinhasReceitasAppDbContext : DbContext
{
    public MinhasReceitasAppDbContext(DbContextOptions options) : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MinhasReceitasAppDbContext).Assembly);
    }
}
