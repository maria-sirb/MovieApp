using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Data;
using MovieAppAPI.Dtos;
using MovieAppAPI.Helper;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;
using System.Data;

namespace MovieAppAPI.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DataContext _context;
        private readonly ISortingHelper<Movie> _sortingHelper;

        public MovieRepository(DataContext context, ISortingHelper<Movie> sortingHelper) 
        {
            _context = context;
            _sortingHelper = sortingHelper;
        }

        public Movie GetMovie(int id)
        {
            return _context.Movies.Where(m => m.MovieId == id).FirstOrDefault();
        }

        public Movie GetMovie(string title)
        {
            return _context.Movies.Where(m => m.Title.ToLower() == title.ToLower()).FirstOrDefault();
        }

        public Director GetMovieDirector(int movieId)
        {
            return _context.Movies.Where(m => m.MovieId == movieId).Select(m => m.Director).FirstOrDefault();
        }

        public ICollection<Actor> GetMovieActors(int id)
        {
           return _context.MovieActors.Where(ma => ma.MovieId == id).Select(ma => ma.Actor).ToList();
        }

        public IQueryable<MovieActor> GetMovieCast(int id)
        {
            return _context.MovieActors.Where(ma => ma.MovieId == id);
        }

        public ICollection<Genre> GetMovieGenres(int id)
        {
            return _context.MovieGenres.Where(mg => mg.MovieId == id).Select(mg => mg.Genre).ToList();
        }
        public ICollection<Movie> GetMovies()
        {
            return _context.Movies.OrderBy(m => m.MovieId).ToList();
        }

        public ICollection<Movie> GetMovies(string? input)
        {
            return _context.Movies.ToList().Where(m => Search(m, input)).OrderByDescending(m => GetSearchScore(m, input)).ToList();
        }
        public PagedResult<Movie> GetMoviesPaged(QueryStringParameters parameters)
        {
            var movies = _sortingHelper.ApplySort(_context.Movies, parameters.OrderBy ?? "");
            return new PagedResult<Movie>(movies, parameters.PageNumber, parameters.PageSize);
        }

        public ICollection<MovieActor> GetRolesInMovie(int id)
        {
            return _context.MovieActors.Where(ma => ma.MovieId == id).ToList();
        }

        public bool MovieExists(int id)
        {
            return _context.Movies.Any(m => m.MovieId == id);
        }
        public bool MovieExists(string title)
        {
            return _context.Movies.Any(m => m.Title.ToLower() == title.ToLower());
        }
        public bool CreateMovie(List<int> genreIds, Movie movie)
        {

            foreach (var genreId in genreIds)
            {
                var movieGenreEntity = _context.Genres.Where(g => g.GenreId == genreId).FirstOrDefault();
                var movieGenre = new MovieGenre()
                {
                    Genre = movieGenreEntity,
                    Movie = movie

                };
                _context.Add(movieGenre);


            }
            _context.Add(movie);
            return Save();

        }

        public bool UpdateMovie(List<int> newGenreIds, Movie movie)
        {

            var movieGenres = GetMovieGenres(movie.MovieId);
            foreach(var genre in movieGenres)
            {  
                var movieGenre = new MovieGenre()
                {
                     Genre = genre,
                     Movie = movie

                };
                _context.MovieGenres.Remove(movieGenre);
       
            }
            _context.SaveChanges();
            foreach(var genreId in newGenreIds)
            {
                var movieGenreEntity = _context.Genres.Where(g => g.GenreId == genreId).FirstOrDefault();
                var movieGenre = new MovieGenre()
                {
                    Genre = movieGenreEntity,
                    Movie = movie

                };
                _context.Add(movieGenre);
            }
            _context.Update(movie);
;            return Save();
        }
        public bool DeleteMovie(Movie movie)
        {
            _context.Remove(movie);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        
        }

        public bool Search(Movie movie, string input)
        {
           
            if (string.IsNullOrEmpty(input))
                return true;
            input = input.ToLower();
            string[] splitInput = input.Split(new char[] { ' ', ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string word in splitInput)
            {
                if(movie.Title.ToLower().Contains(word))
                    return true;
                if (GetMovieDirector(movie.MovieId).Name.ToLower().Contains(word) && word.Length >= 2)
                    return true;
                if (movie.ReleaseYear.ToString().Contains(word) && word.Length >= 2)
                    return true;
                var actors = GetMovieActors(movie.MovieId);
                foreach (Actor actor in actors)
                {
                    if (actor.Name.ToLower().Contains(word) && word.Length >= 2)
                        return true;

                }
            }
          
            return false;
        }
        public int GetSearchScore(Movie movie, string input)
        {
            int searchScore = 0;
            if (string.IsNullOrEmpty(input))
                return 0;
            input = input.ToLower();
            string[] splitInput = input.Split(new char[] { ' ', ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (movie.Title.ToLower().Contains(input))
                searchScore += 5;
            foreach(string word in splitInput)
            {
                if (movie.Title.ToLower().Contains(word))
                      searchScore++;
                if (GetMovieDirector(movie.MovieId).Name.ToLower().Contains(word) && word.Length >= 2)
                    searchScore++;
                if (movie.ReleaseYear.ToString().Contains(word) && word.Length >= 2)
                    searchScore++;
                var actors = GetMovieActors(movie.MovieId);
                foreach (Actor actor in actors)
                {
                    if (actor.Name.ToLower().Contains(word) && word.Length >= 2)
                        searchScore++;
                }

            }
            return searchScore;
        }
    }
}
