using MovieAppAPI.Helper;
using MovieAppAPI.Models;

namespace MovieAppAPI.Interfaces
{
    public interface IActorRepository
    {
        ICollection<Actor> GetActors();
        ICollection<Actor> GetActors(string? input);
        PagedResult<Actor> GetActorsPaged(QueryStringParameters parameters);
        Actor GetActor(int actorId);
        Actor GetActor(string actorName);
        ICollection<Actor> GetActorInAMovie(int movieId);
        ICollection<Movie> GetMovieByActor(int actorId);
        ICollection<MovieActor> GetRolesOfActor(int actorId);
        bool ActorExists(int actorId);
        bool ActorExists(string actorname);
        bool CreateActor(Actor actor);
        bool UpdateActor(Actor actor);
        bool DeleteActor(Actor actor);
        bool Save();
    }
}
