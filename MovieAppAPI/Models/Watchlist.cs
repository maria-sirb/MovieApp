namespace MovieAppAPI.Models
{
    public class Watchlist
    {
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public Movie Movie { get; set; }
        public User User { get; set; }
    }
}
