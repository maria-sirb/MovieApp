using MovieAppAPI.Models;

namespace MovieAppAPI.Interfaces
{
    public interface IMovieActorRepository
    {
        ICollection<MovieActor> GetRoles();
        MovieActor GetRole(int movieId, int actorId);

        bool CreateRole(MovieActor role);
        public bool UpdateRole(MovieActor role);
        public bool DeleteRole(MovieActor role);
        bool RoleExists(int movieId, int actorId);
    }
}
