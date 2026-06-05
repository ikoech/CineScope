using CineScope.Backend.Data;
using CineScope.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class MoviesController : Controller
{
    private readonly CineScopeBackendContext _context;

    public MoviesController(CineScopeBackendContext context)
    {
        _context = context;
    }

    // ============================
    // PUBLIC PAGES (Everyone)
    // ============================

    // Public movie list
    [AllowAnonymous]
    public async Task<IActionResult> Index(string search)
    {
        var movies = from m in _context.Movies
                     select m;

        if (!string.IsNullOrEmpty(search))
        {
            movies = movies.Where(m => m.Title.Contains(search));
        }

        return View(await movies.ToListAsync());
    }

    // Public movie details
    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var movie = await _context.Movies
            .Include(m => m.Reviews).ThenInclude(r => r.User)
            .Include(m => m.Ratings)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null)
            return NotFound();

        return View(movie);
    }

    // ============================
    // ADMIN‑ONLY ACTIONS
    // ============================

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Movie movie)
    {
        if (ModelState.IsValid)
        {
            _context.Add(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
        return View(movie);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
            return NotFound();
        ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
        return View(movie);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, Movie movie)
    {
        if (id != movie.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
        return View(movie);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
            return NotFound();

        return View(movie);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie != null)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
