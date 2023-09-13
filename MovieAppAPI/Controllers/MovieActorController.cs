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
    [ResponseCache(CacheProfileName = "Cache1Min")]
    public class MovieActorController : Controller
    {
        private readonly IMovieActorRepository _movieActorRepository;
        private readonly IMapper _mapper;
        public MovieActorController(IMovieActorRepository movieActorRepository, IMapper mapper)
        {
            _movieActorRepository = movieActorRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MovieActor>))]
        public IActionResult GetRoles()
        {
            var roles = _mapper.Map<List<MovieActorDto>>(_movieActorRepository.GetRoles());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(roles);
        }

        [HttpGet("{movieId}/{actorId}")]
        [ProducesResponseType(200, Type = typeof(MovieActor))]
        [ProducesResponseType(400)]
        public IActionResult GetRole(int movieId, int actorId)
        {
            if (!_movieActorRepository.RoleExists(movieId, actorId))
            {
                return NotFound();
            }
            var role = _mapper.Map<MovieActorDto>(_movieActorRepository.GetRole(movieId, actorId));
            if (!ModelState.IsValid)

            {
                return BadRequest(ModelState);
            }
            return Ok(role);

        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateRole([FromBody] MovieActorDto roleCreate)
        {
            if (roleCreate == null)
                return BadRequest();
            if (_movieActorRepository.RoleExists(roleCreate.MovieId, roleCreate.ActorId))
            {
                return BadRequest("Role already exists.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var roleMap = _mapper.Map<MovieActor>(roleCreate);
            if (!_movieActorRepository.CreateRole(roleMap))
            {
                return BadRequest("Something went wrong while saving.");
            }

            return Ok();

        }
        [HttpPut("{movieId}/{actorId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateRole(int movieId, int actorId, [FromBody] MovieActorDto updatedRole)
        {
            if (updatedRole == null)
                return BadRequest(ModelState);
            if (movieId != updatedRole.MovieId || actorId != updatedRole.ActorId)
                return BadRequest(ModelState);
            if (!_movieActorRepository.RoleExists(movieId, actorId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var roleMap = _mapper.Map<MovieActor>(updatedRole);
            if (!_movieActorRepository.UpdateRole(roleMap))
            {
                return BadRequest("Something went wrong while saving.");

            }
            return NoContent();

        }
        [HttpDelete("{movieId}/{actorId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteRole(int movieId, int actorId)
        {

            if (!_movieActorRepository.RoleExists(movieId, actorId))
                return NotFound();

            var roleToDelete = _movieActorRepository.GetRole(movieId, actorId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_movieActorRepository.DeleteRole(roleToDelete))
            {
                return BadRequest("Something went wrong while deleting role.");
            }
            return NoContent();
        }

    }
}
