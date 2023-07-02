using System.ComponentModel.DataAnnotations;

namespace MovieAppAPI.Models
{
    public class Director
    {
        [Key]
        public int DirectorId { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public DateTime Born { get; set; }
        public int OscarWins { get; set; }
        public int OscarNominations { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
