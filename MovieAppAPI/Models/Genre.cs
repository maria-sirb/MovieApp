using System.ComponentModel.DataAnnotations;

namespace MovieAppAPI.Models
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }
        public string Name { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set;}
    }
}
