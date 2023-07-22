using MovieAppAPI.Models;

namespace MovieAppAPI.Interfaces
{
    public interface IVoteRepository
    {
        ICollection<Vote> GetReviewVotes(int reviewId);
        Vote GetVoteFromUserAndReview(int userId,int reviewId);
        Vote GetVote(int id);
        bool VoteExists(int userId, int reviewId);
        bool VoteExists(int id);
        bool CreateVote(Vote vote);
        bool UpdateVote(Vote vote);
        bool DeleteVote(Vote vote);
        bool Save();
    }
}
