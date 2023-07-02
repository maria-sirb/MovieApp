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
        ICollection<Genre> GetMovieGenres(int id);
        ICollection<Actor> GetMovieActors(int id);
        Director GetMovieDirector(int id);
        bool MovieExists(int id);
        bool MovieExists(string title);
        bool CreateMovie(int actorId, int genreId, string role, Movie movie);
        // public bool CreateMovie(List<(int actorId, string actorRole)> actorsRoles, List<int> genreIds, Movie movie);
        public bool CreateMovie(List<int> genreIds, Movie movie);
        bool UpdateMovie(List<int> genreIds, Movie movie);
        public bool DeleteMovie(Movie movie);

        bool Save();

        bool Search(Movie movie, string input);


    }
}
