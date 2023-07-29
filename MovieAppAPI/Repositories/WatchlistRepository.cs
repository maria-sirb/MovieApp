using MovieAppAPI.Data;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;

namespace MovieAppAPI.Repositories
{
    public class WatchlistRepository : IWatchlistRepository
    {
        private readonly DataContext _context;
        public WatchlistRepository(DataContext context)
        {
            _context = context;
        }
        public bool AddWatchlistItem(Watchlist item)
        {
            _context.Add(item);
            return Save();
        }

        public bool DeleteWatchlistItem(Watchlist item)
        {
            _context.Remove(item);
            return Save();
        }

        public ICollection<Movie> GetUserWatchlist(int userId)
        {
            return _context.Watchlists.Where(w => w.UserId == userId).Select(w => w.Movie).ToList();
        }

        public Watchlist GetWatchlistItem(int userId, int movieId)
        {
            return _context.Watchlists.Where(w => w.UserId == userId && w.MovieId == movieId).FirstOrDefault();
        }

        public bool WatchlistItemExists(int userId, int movieId)
        {
            return _context.Watchlists.Any(w => w.UserId == userId && w.MovieId == movieId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}
