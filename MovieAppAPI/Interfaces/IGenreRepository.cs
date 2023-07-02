using MovieAppAPI.Models;

namespace MovieAppAPI.Interfaces
{
    public interface IGenreRepository
    {
        ICollection<Genre> GetGenres();
        Genre GetGenre(int id);
        Genre GetGenre(string name);
        ICollection<Movie> GetMovieByGenre(int genreId);
        bool GenreExists(int id);
        bool GenreExists(string name);
        bool CreateGenre(Genre genre);
        bool UpdateGenre(Genre genre);
        bool DeleteGenre(Genre genre);
        bool Save();

    }
}
