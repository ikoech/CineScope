using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CineScope.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CineScope.Backend.Models.ViewModels;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly CineScopeBackendContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(CineScopeBackendContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Admin Dashboard
        public async Task<IActionResult> Dashboard()
    {
        var model = new AdminDashboardViewModel
        {
            TotalMovies = await _context.Movies.CountAsync(),
            TotalUsers = await _context.Users.CountAsync(),
            TotalReviews = await _context.Reviews.CountAsync(),
            TotalRatings = await _context.Ratings.CountAsync(),
            TotalFavorites = await _context.FavoriteMovies.CountAsync(),

            NewUsersToday = await _context.Users
                .CountAsync(u => u.CreatedAt.Date == DateTime.Today),

            NewReviewsToday = await _context.Reviews
                .CountAsync(r => r.CreatedAt.Date == DateTime.Today),

            MoviesThisMonth = await _context.Movies
                .CountAsync(m => m.CreatedAt.Month == DateTime.Now.Month &&
                                 m.CreatedAt.Year == DateTime.Now.Year)
        };

        return View(model);
    }
    /*public IActionResult Dashboard()
    {
        return View();
    }
    */
    // Manage Movies
    public async Task<IActionResult> ManageMovies()
    {
        var movies = await _context.Movies.ToListAsync();
        return View(movies);
    }

    // Manage Users
    public async Task<IActionResult> ManageUsers()
    {
        var users = await _userManager.Users.ToListAsync();

        var userList = new List<UserWithRolesViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            userList.Add(new UserWithRolesViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList()
            });
        }

        return View(userList);
    }

    // GET: Admin/EditUser/123
    public async Task<IActionResult> EditUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var allRoles = await _context.Roles.ToListAsync();
        var userRoles = await _userManager.GetRolesAsync(user);

        var vm = new EditUserRolesViewModel
        {
            UserId = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            AvailableRoles = allRoles.Select(r => r.Name).ToList(),
            SelectedRoles = userRoles.ToList()
        };

        return View(vm);
    }

    // POST: Admin/EditUser
    [HttpPost]
    public async Task<IActionResult> EditUser(EditUserRolesViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
            return NotFound();

        var currentRoles = await _userManager.GetRolesAsync(user);

        // Remove all roles first
        await _userManager.RemoveFromRolesAsync(user, currentRoles);

        // Add selected roles
        if (model.SelectedRoles != null)
        {
            await _userManager.AddToRolesAsync(user, model.SelectedRoles);
        }

        return RedirectToAction("ManageUsers");
    }
    // POST: Admin/DeleteUser/123
    [HttpPost]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);

        var vm = new UserWithRolesViewModel
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            Roles = roles.ToList()
        };

        return View(vm);
    }

    // POST: Admin/DeleteUserConfirmed/123
    [HttpPost]
    public async Task<IActionResult> DeleteUserConfirmed(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        // Remove roles first
        var roles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, roles);

        // Delete user
        await _userManager.DeleteAsync(user);

        return RedirectToAction("ManageUsers");
    }
}

public class UserWithRolesViewModel
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public List<string> Roles { get; set; }
}
