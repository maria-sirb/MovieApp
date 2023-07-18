using MovieAppAPI.Models;

namespace MovieAppAPI.Dtos
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public DateTime Date { get; set; }
    }
}
