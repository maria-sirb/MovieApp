using MovieAppAPI.Data;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;

namespace MovieAppAPI.Repositories
{
    public class VoteRepository : IVoteRepository
    {
        private DataContext _context;
        public VoteRepository(DataContext context)
        {
            _context = context;
        }
        public ICollection<Vote> GetReviewVotes(int reviewId)
        {
            return _context.Votes.Where(v => v.Review.ReviewId == reviewId).ToList();
        }
        public Vote GetVoteFromUserAndReview(int userId, int reviewId)
        {
            return _context.Votes.Where(v => v.Review.ReviewId == reviewId && v.User.UserId == userId).FirstOrDefault();
        }
        public bool CreateVote(Vote vote)
        {
            _context.Add(vote);
            return Save();
        }

        public bool DeleteVote(Vote vote)
        {
            _context.Remove(vote);
            return Save();
        }

        public Vote GetVote(int id)
        {
            return _context.Votes.Where(v => v.VoteId == id).FirstOrDefault();
        }

        public bool UpdateVote(Vote vote)
        {
            _context.Update(vote);
            return Save();
        }

        public bool VoteExists(int userId, int reviewId)
        {
            return _context.Votes.Any(v => v.User.UserId == userId && v.Review.ReviewId == reviewId);
        }

        public bool VoteExists(int id)
        {
            return _context.Votes.Any(v => v.VoteId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}
