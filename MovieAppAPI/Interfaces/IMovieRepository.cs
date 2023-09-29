using MovieAppAPI.Models;

namespace MovieAppAPI.Interfaces
{
    public interface IMovieRepository
    {
        ICollection<Movie> GetMovies();
        ICollection<Movie> GetMovies(string? input);
        Movie GetMovie(int id);
        Movie GetMovie(string title);
        ICollection<MovieActor> GetRolesInMovie(int id);
        //ICollection<MovieActor> GetMovieCast(int id);
        IQueryable<MovieActor> GetMovieCast(int id);  
        ICollection<Genre> GetMovieGenres(int id);
        ICollection<Actor> GetMovieActors(int id);
        Director GetMovieDirector(int id);
        bool MovieExists(int id);
        bool MovieExists(string title);
        bool CreateMovie(List<int> genreIds, Movie movie);
        bool UpdateMovie(List<int> genreIds, Movie movie);
        bool DeleteMovie(Movie movie);

        bool Save();

        bool Search(Movie movie, string input);


    }
}
