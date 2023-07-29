using System.ComponentModel.DataAnnotations;

namespace MovieAppAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }  
        public string Email { get; set; }   
        public string Password { get; set; } 
        public string Role { get; set; }
        public string Token { get; set; }
        public ICollection<Vote> Votes { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Watchlist> Watchlist { get; set; }
    }
}
