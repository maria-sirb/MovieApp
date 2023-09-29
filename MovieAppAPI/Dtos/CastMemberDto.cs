namespace MovieAppAPI.Dtos
{
    public class CastMemberDto
    {
        public int ActorId { get; set; }
        public int MovieId { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Role { get; set; }
    }
}
