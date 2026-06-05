
using CineScope.Backend.Data;
using CineScope.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CineScope.Backend.Controllers;

[Authorize]
public class UserSettingsController: Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserSettingsController(UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        var model = new UserSettingsViewModel
        {
            DisplayName = user.UserName,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(UserSettingsViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);

        if (!ModelState.IsValid)
            return View(model);

        user.UserName = model.DisplayName;
        user.Email = model.Email;
        user.AvatarUrl = model.AvatarUrl;

        await _userManager.UpdateAsync(user);
        await _signInManager.RefreshSignInAsync(user);

        ViewBag.Success = "Settings updated successfully";

        return View(model);
    }
}
