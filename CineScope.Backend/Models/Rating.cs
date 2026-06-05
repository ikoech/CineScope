using CineScope.Backend.Data;
using CineScope.Backend.Models;
using System.ComponentModel.DataAnnotations;

public class Rating
{
    public int Id { get; set; }

    [Required]
    public string? UserId { get; set; }

    [Required]
    public int MovieId { get; set; }

    [Range(1, 10)]
    public int Score { get; set; }

    public ApplicationUser? User { get; set; }
    public Movie? Movie { get; set; }
}
