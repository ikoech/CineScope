using CineScope.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineScope.Backend.Controllers;

public class GenresController : Controller
{
    private readonly CineScopeBackendContext _context;

    public GenresController(CineScopeBackendContext context)
    {
        _context = context;
    }

    // PUBLIC: List all genres
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var genres = await _context.Genres
            .OrderBy(g => g.Name)
            .ToListAsync();

        return View(genres);
    }

    // PUBLIC: View movies in a genre
    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var genre = await _context.Genres
            .Include(g => g.Movies)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (genre == null)
            return NotFound();

        return View(genre);
    }

    // ADMIN: Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View();

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(Genre genre)
    {
        if (!ModelState.IsValid)
            return View(genre);

        _context.Genres.Add(genre);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    // ADMIN: Edit
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var genre = await _context.Genres.FindAsync(id);
        return genre == null ? NotFound() : View(genre);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Edit(Genre genre)
    {
        if (!ModelState.IsValid)
            return View(genre);

        _context.Genres.Update(genre);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    // ADMIN: Delete
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var genre = await _context.Genres.FindAsync(id);
        return genre == null ? NotFound() : View(genre);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var genre = await _context.Genres.FindAsync(id);
        if (genre != null)
        {
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}
