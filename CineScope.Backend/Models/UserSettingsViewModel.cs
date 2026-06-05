using System.ComponentModel.DataAnnotations;

namespace CineScope.Backend.Models;

public class UserSettingsViewModel
{
    [Required]
    public string DisplayName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    public string? AvatarUrl { get; set; }
}
