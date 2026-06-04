using CineScope.Backend.Data;
using CineScope.Backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class CineScopeBackendContext : IdentityDbContext<ApplicationUser>
{
    public CineScopeBackendContext(DbContextOptions<CineScopeBackendContext> options)
        : base(options)
    {
    }

    // Your DbSets here
     public DbSet<Movie> Movies { get; set; }
}
