namespace CineScope.Backend.Models.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalMovies { get; set; }
    public int TotalUsers { get; set; }
    public int TotalReviews { get; set; }
    public int TotalRatings { get; set; }
    public int TotalFavorites { get; set; }

    public int NewUsersToday { get; set; }
    public int NewReviewsToday { get; set; }
    public int MoviesThisMonth { get; set; }
}
