using Microsoft.IdentityModel.Tokens;
using MovieAppAPI.Data;
using MovieAppAPI.Helper;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;

namespace MovieAppAPI.Repositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly DataContext _context;
        private readonly ISortingHelper<Actor> _sortingHelper;

        public ActorRepository(DataContext context, ISortingHelper<Actor> sortingHelper)
        {
            _context = context;
            _sortingHelper = sortingHelper;
        }

        public bool ActorExists(int actorId)
        {
            return _context.Actors.Any(a => a.ActorId == actorId);
        }
        public bool ActorExists(string name)
        {
            return _context.Actors.Any(a => a.Name.ToLower() == name.ToLower());
        }

        public Actor GetActor(int actorId)
        {
            return _context.Actors.Where(a => a.ActorId == actorId).FirstOrDefault();
        }

        public Actor GetActor(string name)
        {
            return _context.Actors.Where(a => a.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public ICollection<Actor> GetActorInAMovie(int movieId)
        {
            return _context.MovieActors.Where(ma => ma.Movie.MovieId == movieId).Select(m => m.Actor).ToList();
        }

        public ICollection<Actor> GetActors()
        {
           return _context.Actors.ToList();
        }

        public PagedResult<Actor> GetActorsPaged(QueryStringParameters parameters)
        {
            var sortedActors = _sortingHelper.ApplySort(_context.Actors, parameters.OrderBy??"");
            return new PagedResult<Actor>(sortedActors, parameters.PageNumber, parameters.PageSize);
        }

        public ICollection<Actor> GetActors(string? input)
        {
            return _context.Actors.ToList().Where(a => Search(a, input)).ToList();
        }

        public ICollection<Movie> GetMovieByActor(int actorId)
        {
            return _context.MovieActors.Where(ma => ma.Actor.ActorId == actorId).Select(m => m.Movie).ToList();
        }
        public ICollection<MovieActor> GetRolesOfActor(int actorId)
        {
            return _context.MovieActors.Where(ma => ma.Actor.ActorId == actorId).ToList();
        }
        public bool CreateActor(Actor actor)
        {
            _context.Add(actor);
            return Save();

        }
        public bool UpdateActor(Actor actor)
        {
            _context.Update(actor);
            return Save();
        }
        public bool DeleteActor(Actor actor)
        {
            _context.Remove(actor);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Search(Actor actor , string input)
        {
            if (string.IsNullOrEmpty(input))
                return true;
            input = input.ToLower();
            string[] splitInput = input.Split(new char[] { ' ', ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in splitInput)
            {
                if (actor.Name.ToLower().Contains(word) && word.Length >= 2)
                    return true;
            }
                     
           return false;

        }
    }
}
