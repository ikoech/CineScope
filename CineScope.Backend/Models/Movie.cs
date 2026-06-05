
namespace CineScope.Backend.Models;

public class Movie
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    //public string Genre { get; set; } = string.Empty;

    public int ReleaseYear { get; set; }
    public double Rating { get; set; }
    public int Duration { get; set; }

    public string? PosterUrl { get; set; }
    public string? Description { get; set; }
    public ICollection<Review>? Reviews { get; set; }
    public ICollection<Rating>? Ratings { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int GenreId { get; set; }
    public Genre? Genre { get; set; }

}
