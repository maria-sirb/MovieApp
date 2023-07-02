namespace MovieAppAPI.Dtos
{
    public class DirectorDto
    {
        public int DirectorId { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public DateTime Born { get; set; }
        public int OscarWins { get; set; }
        public int OscarNominations { get; set; }
    }
}
