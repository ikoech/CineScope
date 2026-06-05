using Microsoft.AspNetCore.Identity;

namespace CineScope.Backend.Data;

public class ApplicationUser : IdentityUser
{
    public string? AvatarUrl { get; set; }

    // Needed for Admin Dashboard stats
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
