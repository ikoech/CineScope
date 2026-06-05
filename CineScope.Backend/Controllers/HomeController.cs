using CineScope.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineScope.Backend.Controllers
{
    public class HomeController : Controller
    {
        private readonly CineScopeBackendContext _context;

        public HomeController(CineScopeBackendContext context)
        {
            _context = context;
        }

        // --------------------------------------------------
        // HOME PAGE (with optional genre filtering)
        // --------------------------------------------------
        public async Task<IActionResult> Index(string? genre)
        {
            // Load movies with Genre included
            var movies = _context.Movies
                .Include(m => m.Genre)
                .AsQueryable();

            // Apply genre filter (NEW relational model)
            if (!string.IsNullOrEmpty(genre))
            {
                movies = movies.Where(m => m.Genre.Name == genre);
            }

            // Load distinct genre names for dropdown
            var genres = await _context.Genres
                .Select(g => g.Name)
                .Distinct()
                .ToListAsync();

            // Prepare ViewModel
            var viewModel = new HomeViewModel
            {
                Movies = await movies.ToListAsync(),
                Genres = genres,
                SelectedGenre = genre
            };

            return View(viewModel);
        }
    }

    // --------------------------------------------------
    // VIEWMODEL FOR HOME PAGE
    // --------------------------------------------------
    public class HomeViewModel
    {
        public List<Movie> Movies { get; set; } = new();
        public List<string> Genres { get; set; } = new();
        public string? SelectedGenre { get; set; }
    }
}
