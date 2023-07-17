using MovieAppAPI.Models;

namespace MovieAppAPI.Dtos
{
    public class VoteDto
    {
        public int VoteId { get; set; }
        public bool IsLike { get; set; }
        public bool IsDislike { get; set; }
    }
}
