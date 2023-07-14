using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieAppAPI.Data;
using MovieAppAPI.Dtos;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;
using MovieAppAPI.Repositories;

namespace MovieAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : Controller
    {
        private readonly IActorRepository _actorRepository;
        private readonly IMapper _mapper;
        public ActorController(IActorRepository actorRepository, IMapper mapper)
        {
            _actorRepository = actorRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Actor>))]
        public IActionResult GetActors([FromQuery] string? input)
        {
            var actors = _mapper.Map<List<ActorDto>>(_actorRepository.GetActors(input));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(actors);
        }

        [HttpGet("{actorId}")]
        [ProducesResponseType(200, Type = typeof(Actor))]
        [ProducesResponseType(400)]
        public IActionResult GetActor(int actorId)
        {
            if (!_actorRepository.ActorExists(actorId))
            {
                return NotFound();
            }
            var actor = _mapper.Map<ActorDto>(_actorRepository.GetActor(actorId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(actor);

        }

        [HttpGet("actor-name/{actorName}")]
        [ProducesResponseType(200, Type = typeof(Actor))]
        [ProducesResponseType(400)]
        public IActionResult GetActor(string actorName)
        {
            if (!_actorRepository.ActorExists(actorName))
            {
                return NotFound();
            }
            var actor = _mapper.Map<ActorDto>(_actorRepository.GetActor(actorName));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(actor);

        }

        [HttpGet("movie/{actorId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Movie>))]
        [ProducesResponseType(400)]
        public IActionResult GetMovieByActor( int actorId)
        {
            if(!_actorRepository.ActorExists(actorId))
            {
                return NotFound();

            }
            var movies = _mapper.Map<List<MovieDto>>(_actorRepository.GetMovieByActor(actorId));
            if (!ModelState.IsValid)

            {
                return BadRequest(ModelState);
            }
            return Ok(movies);
        }

        [HttpGet("movieActor/{actorId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Movie>))]
        [ProducesResponseType(400)]
        public IActionResult GetRolesOfActor(int actorId)
        {
            if (!_actorRepository.ActorExists(actorId))
            {
                return NotFound();

            }
            var roles = _mapper.Map<List<MovieActorDto>>(_actorRepository.GetRolesOfActor(actorId));
            if (!ModelState.IsValid)

            {
                return BadRequest(ModelState);
            }
            return Ok(roles);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateActor([FromBody] ActorDto actorCreate)
        {
            if (actorCreate == null)
                return BadRequest();
            var actor = _actorRepository.GetActors()
                        .Where(g => g.Name.Trim().ToUpper() == actorCreate.Name.TrimEnd().ToUpper())
                        .FirstOrDefault();
            if (actor != null)
            {
                return BadRequest("Actor already exists.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var actorMap = _mapper.Map<Actor>(actorCreate);
            if (!_actorRepository.CreateActor(actorMap))
            {
                return BadRequest("Something went wrong while saving.");
            }

            return Ok();

        }

        [HttpPut("{actorId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
         public IActionResult UpdateActor(int actorId, [FromBody] ActorDto updatedActor)
        {
            if (updatedActor == null)
                return BadRequest(ModelState);
            if (actorId != updatedActor.ActorId)
                return BadRequest(ModelState);
            if (!_actorRepository.ActorExists(actorId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var actorMap = _mapper.Map<Actor>(updatedActor);
            if (!_actorRepository.UpdateActor(actorMap))
            {
                return BadRequest("Something went wrong while saving.");
            }
            return NoContent();

        }
        [HttpDelete("{actorId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteActor(int actorId)
        {

            if (!_actorRepository.ActorExists(actorId))
                return NotFound();
            var actorToDelete = _actorRepository.GetActor(actorId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_actorRepository.DeleteActor(actorToDelete))
            {
                return BadRequest("Something went wrong while deleting actor.");

            }
            return NoContent();
        }
    }

}
