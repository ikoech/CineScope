using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AdminReviewsController : Controller
{
    private readonly CineScopeBackendContext _context;

    public AdminReviewsController(CineScopeBackendContext context)
    {
        _context = context;
    }

    // List all reviews
    public async Task<IActionResult> Index()
    {
        var reviews = await _context.Reviews
            .Include(r => r.Movie)
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return View(reviews);
    }

    // Delete review
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var review = await _context.Reviews.FindAsync(id);

        if (review == null)
            return NotFound();

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
