using System.ComponentModel.DataAnnotations;

namespace MovieAppAPI.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public DateTime Date { get; set; }
        public User? User { get; set; }
        public Movie Movie { get; set; }
        public ICollection<Vote> Votes { get; set; }

    }
}
