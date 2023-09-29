using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieAppAPI.Dtos;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;
using MovieAppAPI.Repositories;

namespace MovieAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ResponseCache(CacheProfileName = "CacheDefault")]
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IDirectorRepository _directorRepository;
        private readonly IMapper _mapper;
        public MovieController(IMovieRepository movieRepository, IDirectorRepository directorRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _directorRepository = directorRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Movie>))]
        public IActionResult GetMovies([FromQuery] string? input)
        {
            var movies = _mapper.Map<List<MovieDto>>(_movieRepository.GetMovies(input));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(movies);
        }

        [HttpGet("{movieId}")]
        [ProducesResponseType(200, Type = typeof(Movie))]
        [ProducesResponseType(400)]
        public IActionResult GetMovie(int movieId)
        {
            if(!_movieRepository.MovieExists(movieId))
            {
                return NotFound();
            }
            var movie = _mapper.Map<MovieDto>(_movieRepository.GetMovie(movieId));
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(movie);

        }
        [HttpGet("movie-title/{movieTitle}")]
        [ProducesResponseType(200, Type = typeof(Movie))]
        [ProducesResponseType(400)]
        public IActionResult GetMovie(string movieTitle)
        {
            if (!_movieRepository.MovieExists(movieTitle))
            {
                return NotFound();
            }
            var movie = _mapper.Map<MovieDto>(_movieRepository.GetMovie(movieTitle));
            if (!ModelState.IsValid)

            {
                return BadRequest(ModelState);
            }
            return Ok(movie);

        }

        [HttpGet("movieActor/{movieId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MovieActor>))]
        [ProducesResponseType(400)]
        public IActionResult GetRolesInMovie(int movieId)
        {
            if (!_movieRepository.MovieExists(movieId))
            {
                return NotFound();

            }
            var roles = _mapper.Map<List<MovieActorDto>>(_movieRepository.GetRolesInMovie(movieId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(roles);
        }

        [HttpGet("cast/{movieId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CastMemberDto>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult GetMovieCast(int movieId)
        {
            if (!_movieRepository.MovieExists(movieId))
                return NotFound();
            var cast = _mapper.ProjectTo<CastMemberDto>(_movieRepository.GetMovieCast(movieId)).ToList();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(cast);
        }

        [HttpGet("actor/{movieId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Actor>))]
        [ProducesResponseType(400)]
        public IActionResult GetActorsInMovie(int movieId)
        {
            if (!_movieRepository.MovieExists(movieId))
            {
                return NotFound();

            }
            var actors = _mapper.Map<List<ActorDto>>(_movieRepository.GetMovieActors(movieId));
            if (!ModelState.IsValid)

            {
                return BadRequest(ModelState);
            }
            return Ok(actors);
        }

        [HttpGet("genre/{movieId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Actor>))]
        [ProducesResponseType(400)]
        public IActionResult GetMovieGenres(int movieId)
        {
            if (!_movieRepository.MovieExists(movieId))
            {
                return NotFound();

            }
            var genres = _mapper.Map<List<GenreDto>>(_movieRepository.GetMovieGenres(movieId));
            if (!ModelState.IsValid)

            {
                return BadRequest(ModelState);
            }
            return Ok(genres);
        }

        [HttpGet("director/{movieId}")]
        [ProducesResponseType(200, Type = typeof(Director))]
        [ProducesResponseType(400)]
        public IActionResult GetDirectorOfAMovie(int movieId)
        {
            var director = _mapper.Map<DirectorDto>(_movieRepository.GetMovieDirector(movieId));
            if (!ModelState.IsValid)

            {
                return BadRequest(ModelState);
            }
            return Ok(director);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateMovie([FromQuery] int directorId, [FromQuery]List<int> genreIds, [FromBody] MovieDto movieCreate)
        {
            if (movieCreate == null)
                return BadRequest();
            if (_movieRepository.MovieExists(movieCreate.Title))
            {
                return BadRequest("Movie already exists.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var movieMap = _mapper.Map<Movie>(movieCreate);
            movieMap.Director = _directorRepository.GetDirector(directorId);

            if (!_movieRepository.CreateMovie(genreIds, movieMap))
            {
                return BadRequest("Something went wrong while saving.");
            }

            return Ok(movieMap.MovieId);

        }

        [HttpPut("{movieId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateMovie(int movieId, [FromQuery] int directorId, [FromQuery] List<int> genreIds,[FromBody] MovieDto updatedMovie)
        {
            if (updatedMovie == null)
                return BadRequest(ModelState);
            if (movieId != updatedMovie.MovieId)
                return BadRequest(ModelState);
            if (!_movieRepository.MovieExists(movieId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movieMap = _mapper.Map<Movie>(updatedMovie);
            movieMap.Director = _directorRepository.GetDirector(directorId);
            if (!_movieRepository.UpdateMovie(genreIds, movieMap))
            {
                return BadRequest("Something went wrong while saving.");
            }
            return NoContent();

        }
        [HttpDelete("{movieId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteMovie(int movieId)
        {

            if (!_movieRepository.MovieExists(movieId))
                return NotFound();
          
            var movieToDelete = _movieRepository.GetMovie(movieId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_movieRepository.DeleteMovie(movieToDelete))
            {
                return BadRequest("Something went wrong while deleting movie.");

            }
            return NoContent();
        }
    }

}
