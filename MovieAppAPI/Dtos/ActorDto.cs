namespace MovieAppAPI.Dtos
{
    public class ActorDto
    {
        public int ActorId { get; set; }
        public string Name { get; set; }
        public string? Photo { get; set; }
        public DateTime? Born { get; set; }
        public int? OscarWins { get; set; }
        public int? OscarNominations { get; set; }
    }
}
