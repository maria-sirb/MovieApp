using MovieAppAPI.Data;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;

namespace MovieAppAPI.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly DataContext _context;

        public GenreRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Genre> GetGenres()
        {
            return _context.Genres.ToList();
        }

        public Genre GetGenre(int id)
        {
            return _context.Genres.Where(g => g.GenreId == id).FirstOrDefault();
        }

       public Genre GetGenre(string name)
       {
            return _context.Genres.Where(g => g.Name.ToLower() == name.ToLower()).FirstOrDefault();
       }

        public ICollection<Movie> GetMovieByGenre(int genreId)
        {
            return _context.MovieGenres.Where(mg => mg.GenreId == genreId).Select(g => g.Movie).ToList();
        }

        public bool GenreExists(int id)
        {
            return _context.Genres.Any(g => g.GenreId == id);
        }
        public bool GenreExists(string name)
        {
            return _context.Genres.Any(g => g.Name.ToLower() == name.ToLower());
        }

        public bool CreateGenre(Genre genre)
        {
            _context.Add(genre);
            return Save();

        }
        public bool UpdateGenre(Genre genre)
        {
            _context.Update(genre);
            return Save();
        }

        public bool DeleteGenre(Genre genre)
        {
            _context.Remove(genre);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
