using CineScope.Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineScope.Backend.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
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
