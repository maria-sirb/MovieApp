using MovieAppAPI.Helper;
using MovieAppAPI.Models;

namespace MovieAppAPI.Interfaces
{
    public interface IDirectorRepository
    {
        ICollection<Director> GetDirectors();
        ICollection<Director> GetDirectors(string? input);
        PagedResult<Director> GetDirectorsPaged(QueryStringParameters parameters);
        Director GetDirector(int id);
        Director GetDirector(string name);
        ICollection<Movie> GetMoviesFromADirector(int directorId);
        bool DirectorExists(int id);
        bool DirectorExists(string name);
        bool CreateDirector(Director director);
        bool UpdateDirector(Director director); 
        bool DeleteDirector(Director director);    
        bool Save();
    }

}

