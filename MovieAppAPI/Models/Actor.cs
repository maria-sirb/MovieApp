using System.ComponentModel.DataAnnotations;

namespace MovieAppAPI.Models
{
    public class Actor
    {
        [Key]
        public int ActorId { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public DateTime Born {get; set;}
        public int OscarWins { get; set; }
        public int OscarNominations { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; }
    }
}
