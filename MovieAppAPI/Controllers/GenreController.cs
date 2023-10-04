
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
    public class GenreController : Controller
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        public GenreController(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;

            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GenreDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetGenres()
        {
            var genres = _mapper.Map<List<GenreDto>>(_genreRepository.GetGenres());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(genres);
        }
        [HttpGet("{genreId}")]
        [ProducesResponseType(200, Type = typeof(GenreDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult GetGenre(int genreId)
        {
            if (!_genreRepository.GenreExists(genreId))
            {
                return NotFound();
            }
            var genre = _mapper.Map<GenreDto>(_genreRepository.GetGenre(genreId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(genre);

        }
        [HttpGet("genre-name/{genreName}")]
        [ProducesResponseType(200, Type = typeof(GenreDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetGenreByName(string genreName)
        {
            if (!_genreRepository.GenreExists(genreName))
            {
                return NotFound();
            }
            var genre = _mapper.Map<GenreDto>(_genreRepository.GetGenre(genreName));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(genre);

        }
        [HttpGet("movie/{genreId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MovieDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetMovieByGenreId(int genreId)
        {
            var movies = _mapper.Map<List<MovieDto>>(_genreRepository.GetMovieByGenre(genreId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(movies);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateGenre([FromBody] GenreDto genreCreate)
        {
            if (genreCreate == null)
                return BadRequest();
            var genre = _genreRepository.GetGenres()
                        .Where(g => g.Name.Trim().ToUpper() == genreCreate.Name.TrimEnd().ToUpper())
                        .FirstOrDefault();
            if (genre != null)
            {
                ModelState.AddModelError("", "Genre already exits");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var genreMap = _mapper.Map<Genre>(genreCreate);
            if (!_genreRepository.CreateGenre(genreMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok();

        }
        [HttpPut("{genreId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult  UpdateGenre(int genreId, [FromBody]GenreDto updatedGenre)
        {
            if (updatedGenre == null)
                return BadRequest(ModelState);
            if (genreId != updatedGenre.GenreId)
                return BadRequest(ModelState);
            if (!_genreRepository.GenreExists(genreId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var genreMap = _mapper.Map<Genre>(updatedGenre);
            if(!_genreRepository.UpdateGenre(genreMap))
            {
                ModelState.AddModelError("", "Something went wrong updating genre");
                return StatusCode(500, ModelState); 
                 
            }
            return NoContent();

        }

        [HttpDelete("{genreId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteGenre(int genreId)
        {
           
            if (!_genreRepository.GenreExists(genreId))
                return NotFound();
           
            var genreToDelete = _genreRepository.GetGenre(genreId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_genreRepository.DeleteGenre(genreToDelete))
            {
                ModelState.AddModelError("","Something went wrong deleting genre.");
                
            }
            return NoContent();
        }
    }
}
