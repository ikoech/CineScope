using CineScope.Backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Member")]
public class RatingsController : Controller
{
    private readonly CineScopeBackendContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public RatingsController(CineScopeBackendContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Rate(int movieId, int score)
    {
        var user = await _userManager.GetUserAsync(User);

        var existing = await _context.Ratings
            .FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == user.Id);

        if (existing == null)
        {
            _context.Ratings.Add(new Rating
            {
                MovieId = movieId,
                UserId = user.Id,
                Score = score
            });
        }
        else
        {
            existing.Score = score;
            _context.Ratings.Update(existing);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Movies", new { id = movieId });
    }
}
