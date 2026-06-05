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

    //DbSets here
     public DbSet<Movie> Movies { get; set; }
     public DbSet<FavoriteMovie> FavoriteMovies { get; set; }
     public DbSet<Review> Reviews { get; set; }
     public DbSet<Rating> Ratings { get; set; }

}
