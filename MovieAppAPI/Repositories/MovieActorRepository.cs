using MovieAppAPI.Data;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;

namespace MovieAppAPI.Repositories
{
    public class MovieActorRepository : IMovieActorRepository
    {

        private readonly DataContext _context;

        public MovieActorRepository(DataContext context)
        {
            _context = context;
        }

        public MovieActor GetRole(int movieId, int actorId)
        {
            return _context.MovieActors.Where(ma => ma.MovieId == movieId && ma.ActorId == actorId).FirstOrDefault();
        }

        public ICollection<MovieActor> GetRoles()
        {
            return _context.MovieActors.ToList();
        }
        public bool CreateRole(MovieActor role)
        {
            _context.Add(role);
            return Save();
        }
        public bool UpdateRole(MovieActor role)
        {
            _context.Update(role);
            return Save();
        }
        public bool DeleteRole(MovieActor role)
        {
            _context.Remove(role);
            return Save();
        }

        public bool RoleExists(int movieId, int actorId)
        {
            return _context.MovieActors.Any(ma => ma.MovieId == movieId && ma.ActorId == actorId);
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
