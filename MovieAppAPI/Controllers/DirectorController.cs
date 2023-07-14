
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
    public class DirectorController : Controller
    {
        private readonly IDirectorRepository _directorRepository;
        private readonly IMapper _mapper;
        public DirectorController(IDirectorRepository directorRepository, IMapper mapper)
        {
            _directorRepository = directorRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Director>))]

        public IActionResult GetDirectors([FromQuery] string? input)
        {
            var directors = _mapper.Map<List<DirectorDto>>(_directorRepository.GetDirectors(input));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(directors);
        }

        [HttpGet("{directorId}")]
        [ProducesResponseType(200, Type = typeof(Director))]
        [ProducesResponseType(400)]
        public IActionResult GetDirector(int directorId)
        {
            if (!_directorRepository.DirectorExists(directorId))
            {
                return NotFound();
            }
            var director = _mapper.Map<DirectorDto>(_directorRepository.GetDirector(directorId));
            if (!ModelState.IsValid)

            {
                return BadRequest(ModelState);
            }
            return Ok(director);

        }
        [HttpGet("director-name/{directorName}")]
        [ProducesResponseType(200, Type = typeof(Director))]
        [ProducesResponseType(400)]
        public IActionResult GetDirector(string directorName)
        {
            if (!_directorRepository.DirectorExists(directorName))
            {
                return NotFound();
            }
            var director = _mapper.Map<DirectorDto>(_directorRepository.GetDirector(directorName));
            if (!ModelState.IsValid)

            {
                return BadRequest(ModelState);
            }
            return Ok(director);

        }

        [HttpGet("/movie/{directorId}")]
        [ProducesResponseType(200, Type = typeof(Movie))]
        [ProducesResponseType(400)]
        public IActionResult GetDirectorMovie(int directorId)
        {
            var movies = _mapper.Map<List<MovieDto>>(_directorRepository.GetMoviesFromADirector(directorId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(movies);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateDirector([FromBody] DirectorDto directorCreate)
        {
            if (directorCreate == null)
                return BadRequest();
            var director = _directorRepository.GetDirectors()
                        .Where(d => d.Name.Trim().ToUpper() == directorCreate.Name.TrimEnd().ToUpper())
                        .FirstOrDefault();
            if (director != null)
            {
                return BadRequest("Director already exists.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var directorMap = _mapper.Map<Director>(directorCreate);
            if (!_directorRepository.CreateDirector(directorMap))
            {
                return BadRequest("Something went wrong while saving.");
            }

            return Ok();

        }
        [HttpPut("{directorId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateDirector(int directorId, [FromBody] DirectorDto updatedDirector)
        {
            if (updatedDirector == null)
                return BadRequest(ModelState);
            if (directorId != updatedDirector.DirectorId)
                return BadRequest(ModelState);
            if (!_directorRepository.DirectorExists(directorId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var directorMap = _mapper.Map<Director>(updatedDirector);
            if (!_directorRepository.UpdateDirector(directorMap))
            {
                return BadRequest("Something went wrong while saving.");

            }
            return NoContent();



        }
        [HttpDelete("{directorId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteDirector(int directorId)
        {

            if (!_directorRepository.DirectorExists(directorId))
                return NotFound();
           
            var directorToDelete = _directorRepository.GetDirector(directorId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_directorRepository.DeleteDirector(directorToDelete))
            {
                return BadRequest("Something went wrong while deleting director.");

            }
            return NoContent();
        }
    }
}
