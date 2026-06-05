using CineScope.Backend.Data;
using CineScope.Backend.Models;
using System.ComponentModel.DataAnnotations;

public class FavoriteMovie
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public int MovieId { get; set; }

    public ApplicationUser User { get; set; }
    public Movie Movie { get; set; }
}
