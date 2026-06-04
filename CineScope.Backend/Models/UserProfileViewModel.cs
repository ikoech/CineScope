namespace CineScope.Backend.Models;

public class UserProfileViewModel
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public IList<string> Roles { get; set; }
}
