using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAppAPI.Dtos;
using MovieAppAPI.Helper;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;
using MovieAppAPI.Repositories;
using Newtonsoft.Json;
using System.Data;

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
        [ProducesResponseType(200, Type = typeof(IEnumerable<MovieDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetMovies([FromQuery] string? input)
        {
            var movies = _mapper.Map<List<MovieDto>>(_movieRepository.GetMovies(input));
            return Ok(movies);
        }

        [HttpGet("paged")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MovieDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetMovies([FromQuery] QueryStringParametersMovie parameters)
        {
            var pagedResult = _movieRepository.GetMoviesPaged(parameters);
            var movies = _mapper.Map<List<MovieDto>>(pagedResult.items);
            var paginationData = _mapper.Map<PaginationDataDto>(pagedResult);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationData));
            return Ok(movies);
        }

        [HttpGet("{movieId}")]
        [ProducesResponseType(200, Type = typeof(MovieDto))]
        [ProducesResponseType(400)]
        public IActionResult GetMovie(int movieId)
        {
            if(!_movieRepository.MovieExists(movieId))
            {
                return NotFound();
            }
            var movie = _mapper.Map<MovieDto>(_movieRepository.GetMovie(movieId));
            return Ok(movie);

        }
        [HttpGet("movie-title/{movieTitle}")]
        [ProducesResponseType(200, Type = typeof(MovieDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetMovie(string movieTitle)
        {
            if (!_movieRepository.MovieExists(movieTitle))
            {
                return NotFound();
            }
            var movie = _mapper.Map<MovieDto>(_movieRepository.GetMovie(movieTitle));
            return Ok(movie);

        }

        [HttpGet("movieActor/{movieId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MovieActorDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetRolesInMovie(int movieId)
        {
            if (!_movieRepository.MovieExists(movieId))
            {
                return NotFound();
            }
            var roles = _mapper.Map<List<MovieActorDto>>(_movieRepository.GetRolesInMovie(movieId));
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
            return Ok(cast);
        }

        [HttpGet("actor/{movieId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ActorDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetActorsInMovie(int movieId)
        {
            if (!_movieRepository.MovieExists(movieId))
            {
                return NotFound();
            }
            var actors = _mapper.Map<List<ActorDto>>(_movieRepository.GetMovieActors(movieId));
            return Ok(actors);
        }

        [HttpGet("genre/{movieId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GenreDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetMovieGenres(int movieId)
        {
            if (!_movieRepository.MovieExists(movieId))
            {
                return NotFound();
            }
            var genres = _mapper.Map<List<GenreDto>>(_movieRepository.GetMovieGenres(movieId));
            return Ok(genres);
        }

        [HttpGet("director/{movieId}")]
        [ProducesResponseType(200, Type = typeof(DirectorDto))]
        [ProducesResponseType(400)]
        public IActionResult GetDirectorOfAMovie(int movieId)
        {
            var director = _mapper.Map<DirectorDto>(_movieRepository.GetMovieDirector(movieId));
            return Ok(director);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        [Authorize(Roles = "admin")]
        public IActionResult CreateMovie([FromQuery] int directorId, [FromQuery]List<int> genreIds, [FromBody] MovieDto movieCreate)
        {
            if (movieCreate == null)
                return BadRequest();
            if (_movieRepository.MovieExists(movieCreate.Title))
            {
                return BadRequest("Movie already exists.");
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
        [Authorize(Roles = "admin")]
        public IActionResult UpdateMovie(int movieId, [FromQuery] int directorId, [FromQuery] List<int> genreIds,[FromBody] MovieDto updatedMovie)
        {
            if (updatedMovie == null)
                return BadRequest(ModelState);
            if (movieId != updatedMovie.MovieId)
                return BadRequest(ModelState);
            if (!_movieRepository.MovieExists(movieId))
                return NotFound();

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
        [Authorize(Roles = "admin")]
        public IActionResult DeleteMovie(int movieId)
        {

            if (!_movieRepository.MovieExists(movieId))
                return NotFound();
          
            var movieToDelete = _movieRepository.GetMovie(movieId);
            if (!_movieRepository.DeleteMovie(movieToDelete))
            {
                return BadRequest("Something went wrong while deleting movie.");
            }
            return NoContent();
        }
    }

}
