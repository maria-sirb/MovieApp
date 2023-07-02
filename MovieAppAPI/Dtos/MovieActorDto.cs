using MovieAppAPI.Models;

namespace MovieAppAPI.Dtos
{
    public class MovieActorDto
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public string Role { get; set; }
    }
}
