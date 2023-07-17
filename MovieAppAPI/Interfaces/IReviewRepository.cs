using MovieAppAPI.Models;

namespace MovieAppAPI.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int id);
        ICollection<Review> GetReviewsFromMovie(int movieId);
        ICollection<Review> GetReviewsFromUser(int userId);IColle
        bool CreateReview(Review review);
        bool DeleteReview(Review review);
        bool Save();
    }
}
