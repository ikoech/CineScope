using CineScope.Backend.Data;
using CineScope.Backend.Models;
using System;
using System.ComponentModel.DataAnnotations;

public class Review
{
    public int Id { get; set; }

    [Required]
    public string? UserId { get; set; }

    [Required]
    public int MovieId { get; set; }

    [Required]
    [StringLength(500)]
    public string? Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ApplicationUser? User { get; set; }
    public Movie? Movie { get; set; }
}
