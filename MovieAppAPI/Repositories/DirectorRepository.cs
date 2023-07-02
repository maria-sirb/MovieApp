using AutoMapper;
using MovieAppAPI.Data;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;

namespace MovieAppAPI.Repositories
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly DataContext _context;
       

        public DirectorRepository(DataContext context)
        {
            _context = context;
          
        }

        public bool CreateDirector(Director director)
        {
            _context.Add(director);
            return Save();
        }

        public bool DirectorExists(int id)
        {
            return _context.Directors.Any(d => d.DirectorId == id);

        }
        public bool DirectorExists(string name)
        {
            return _context.Directors.Any(d => d.Name.ToLower() == name.ToLower());
        }

        public Director GetDirector(int id)
        {
            return _context.Directors.Where(d => d.DirectorId == id).FirstOrDefault();
        }

        public Director GetDirector(string name)
        {
            return _context.Directors.Where(d => d.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public ICollection<Director> GetDirectors()
        {
            return _context.Directors.ToList();
        }
        public ICollection<Director> GetDirectors(string? input)
        {
            return _context.Directors.ToList().Where(d => Search(d, input)).ToList();
        }

        public ICollection<Movie> GetMoviesFromADirector(int directorId)
        {
            return _context.Movies.Where(m => m.Director.DirectorId == directorId).ToList();
        }
        public bool UpdateDirector(Director director)
        {
            _context.Update(director);
            return Save();
        }
        public bool RemoveMovieFromDirector(Director director, Movie movie) 
        {
            var movies = GetMoviesFromADirector(director.DirectorId);
            if (movies.Contains(movie))
                movies.Remove(movie);
            director.Movies= movies;
            return Save();
        }
        public bool DeleteDirector(Director director)
        {
            _context.Remove(director);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public bool Search(Director director, string input)
        {
            if (string.IsNullOrEmpty(input))
                return true;
            input = input.ToLower();
            string[] splitInput = input.Split(new char[] { ' ', ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in splitInput)
            {
                if (director.Name.ToLower().Contains(word) && word.Length >= 2)
                    return true;
            }

            return false;

        }
    }
}
