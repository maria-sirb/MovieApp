using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAppAPI.Models
{
    public class Vote
    {
        [Key]
        public int VoteId { get; set; }
        public Review Review { get; set; }
        public User? User { get; set; }
        public bool IsLike { get; set; }
        public bool IsDislike { get; set; }
        
    }
}
