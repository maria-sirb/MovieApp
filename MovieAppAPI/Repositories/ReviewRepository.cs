using MovieAppAPI.Data;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;

namespace MovieAppAPI.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;
        public ReviewRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);
            return Save();
        }

        public Review GetReview(int id)
        {
           return  _context.Reviews.Where(r => r.ReviewId == id).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.OrderBy(r => r.ReviewId).ToList();
        }

        public ICollection<Review> GetReviewsFromMovie(int movieId)
        {
            return _context.Reviews.Where(r => r.Movie.MovieId == movieId).ToList();
        }

        public ICollection<Review> GetReviewsFromUser(int userId)
        {
            return _context.Reviews.Where(r => r.User.UserId == userId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
