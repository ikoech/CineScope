using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CineScope.Backend.Data;
using CineScope.Backend.Models;

namespace CineScope.Backend.Controllers;

public class UserController: Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);

        var model = new UserProfileViewModel
        {
            Email = user.Email,
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber,
            Roles = await _userManager.GetRolesAsync(user)
        };

        return View(model);
    }
}
