using MovieAppAPI.Models;

namespace MovieAppAPI.Interfaces
{
    public interface IWatchlistRepository
    {
        ICollection<Movie> GetUserWatchlist(int userId);
        Watchlist GetWatchlistItem(int userId, int movieId);
        bool WatchlistItemExists(int userId, int movieId);
        bool AddWatchlistItem(Watchlist item);
        bool DeleteWatchlistItem(Watchlist item);
        bool Save();
    }
}
