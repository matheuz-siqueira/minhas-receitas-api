using Microsoft.EntityFrameworkCore;
using MinhasReceitasApp.Domain.Entities;

namespace MinhasReceitasApp.Infrastructure.DataAccess;

public class MinhasReceitasAppDbContext : DbContext
{
    public MinhasReceitasAppDbContext(DbContextOptions options) : base (options)
    { } 

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.ApplyConfigurationsFromAssembly(typeof(MinhasReceitasAppDbContext).Assembly); 
    }
}
