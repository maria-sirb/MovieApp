using MovieAppAPI.Models;

namespace MovieAppAPI.Dtos
{
    public class MovieDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string? Poster { get; set; }
        public int? RunTime { get; set; }
        public int? ReleaseYear { get; set; }
        public string? Summary { get; set; }
        public double? ImdbRating { get; set; }
        public int? OscarWins { get; set; }
        public int? OscarNominations { get; set; }
        public string? StoryLine { get; set; }
        
    }
}
