using CineScope.Backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Member")]
public class FavoritesController : Controller
{
    private readonly CineScopeBackendContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public FavoritesController(CineScopeBackendContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Add to favorites
    [HttpPost]
    public async Task<IActionResult> Add(int movieId)
    {
        var user = await _userManager.GetUserAsync(User);

        bool exists = await _context.FavoriteMovies
            .AnyAsync(f => f.MovieId == movieId && f.UserId == user.Id);

        if (!exists)
        {
            _context.FavoriteMovies.Add(new FavoriteMovie
            {
                MovieId = movieId,
                UserId = user.Id
            });

            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Details", "Movies", new { id = movieId });
    }

    // Remove from favorites
    [HttpPost]
    public async Task<IActionResult> Remove(int movieId)
    {
        var user = await _userManager.GetUserAsync(User);

        var fav = await _context.FavoriteMovies
            .FirstOrDefaultAsync(f => f.MovieId == movieId && f.UserId == user.Id);

        if (fav != null)
        {
            _context.FavoriteMovies.Remove(fav);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("MyFavorites");
    }

    // List favorites
    public async Task<IActionResult> MyFavorites()
    {
        var user = await _userManager.GetUserAsync(User);

        var favorites = await _context.FavoriteMovies
            .Where(f => f.UserId == user.Id)
            .Include(f => f.Movie)
            .ToListAsync();

        return View(favorites);
    }
}
