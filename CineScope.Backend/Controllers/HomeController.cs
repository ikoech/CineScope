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
            // Start with all movies
            var movies = _context.Movies.AsQueryable();

            // Apply genre filter if selected
            if (!string.IsNullOrEmpty(genre))
            {
                movies = movies.Where(m => m.Genre == genre);
            }

            // Load distinct genres for dropdown/filter UI
            var genres = await _context.Movies
                .Select(m => m.Genre)
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


/*using CineScope.Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineScope.Backend.Controllers;

public class HomeController : Controller
{
    private readonly CineScopeBackendContext _context;

    public HomeController(CineScopeBackendContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? genre)
    {
        // Start with all movies
        var movies = _context.Movies.AsQueryable();

        // Apply genre filter if selected
        if (!string.IsNullOrEmpty(genre))
        {
            movies = movies.Where(m => m.Genre == genre);
        }

        // Load movies ordered by rating
        var movieList = await movies
            .OrderByDescending(m => m.Rating)
            .ToListAsync();

        // Pass selected genre to the view
        ViewBag.SelectedGenre = genre;

        return View(movieList);
    }
}
*/