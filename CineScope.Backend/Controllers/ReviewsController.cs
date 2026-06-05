using CineScope.Backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Member")]
public class ReviewsController : Controller
{
    private readonly CineScopeBackendContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReviewsController(CineScopeBackendContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // POST: Add Review
    [HttpPost]
    public async Task<IActionResult> Add(int movieId, string content)
    {
        var user = await _userManager.GetUserAsync(User);

        if (string.IsNullOrWhiteSpace(content))
            return RedirectToAction("Details", "Movies", new { id = movieId });

        var review = new Review
        {
            MovieId = movieId,
            UserId = user.Id,
            Content = content
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Movies", new { id = movieId });
    }

    // POST: Delete Review
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userManager.GetUserAsync(User);

        var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);

        if (review == null || review.UserId != user.Id)
            return Unauthorized();

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Movies", new { id = review.MovieId });
    }
}
