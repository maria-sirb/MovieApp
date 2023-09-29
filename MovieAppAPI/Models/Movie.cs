using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAppAPI.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int? RunTime { get; set; }
        public int? ReleaseYear { get; set; }
        public string? Summary { get; set; }
        public string? Poster { get; set; }

        public double? ImdbRating { get; set; }
        public int? OscarWins { get; set; }
        public int? OscarNominations { get; set; }
        public string? StoryLine { get; set; }
        public Director Director { get; set; }
        public ICollection<MovieGenre> MovieGenres {get; set;}
        public ICollection<MovieActor> MovieActors { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Watchlist> Watchlist { get; set; }
       
    }
}
