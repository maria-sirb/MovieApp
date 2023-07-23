using MovieAppAPI.Models;

namespace MovieAppAPI.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int id);
        ICollection<Review> GetReviewsFromMovie(int movieId);
        double GetAverageRatingFromMovie(int movieId);
        ICollection<Review> GetReviewsFromUser(int userId);
        User GetReviewAuthor(int reviewId);
        bool ReviewExists(int reviewId);
        bool ReviewExists(int userId, int movieId);
        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool Save();
    }
}
