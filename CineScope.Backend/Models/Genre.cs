using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CineScope.Backend.Models;

public class Genre
{
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    public ICollection<Movie>? Movies { get; set; }
}
