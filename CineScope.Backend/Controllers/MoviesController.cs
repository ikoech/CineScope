using CineScope.Backend.Data;
using CineScope.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class MoviesController : Controller
{
    private readonly CineScopeBackendContext _context;
    private readonly TmdbService _tmdb;

    public MoviesController(CineScopeBackendContext context, TmdbService tmdb)
    {
        _context = context;
        _tmdb = tmdb;
    }

    // ============================
    // PUBLIC PAGES (Everyone)
    // ============================

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

        // ⭐ Fetch extra images
        ViewBag.ExtraImages = await _tmdb.GetExtraImagesAsync(movie.Title);

        return View(movie);
    }

    // ============================
    // ADMIN‑ONLY ACTIONS
    // ============================

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Movie movie)
    {
        if (ModelState.IsValid)
        {
            // ⭐ Auto‑fetch poster if empty
            if (string.IsNullOrWhiteSpace(movie.PosterUrl))
            {
                var autoPoster = await _tmdb.GetPosterUrlAsync(movie.Title);
                if (autoPoster != null)
                    movie.PosterUrl = autoPoster;
            }

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
            // ⭐ Auto‑fetch poster if empty
            if (string.IsNullOrWhiteSpace(movie.PosterUrl))
            {
                var autoPoster = await _tmdb.GetPosterUrlAsync(movie.Title);
                if (autoPoster != null)
                    movie.PosterUrl = autoPoster;
            }

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
