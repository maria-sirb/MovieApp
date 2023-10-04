using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieAppAPI.Data;
using MovieAppAPI.Dtos;
using MovieAppAPI.Helper;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;
using MovieAppAPI.Repositories;
using Newtonsoft.Json;

namespace MovieAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ResponseCache(CacheProfileName = "CacheDefault")]
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<ActorDto>))]
        public IActionResult GetActors([FromQuery] string? input)
        {
            var actors = _mapper.Map<List<ActorDto>>(_actorRepository.GetActors(input));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(actors);
        }

        [HttpGet("paged")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ActorDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetActors([FromQuery] QueryStringParameters parameters)
        {
            var pagedResult = _actorRepository.GetActorsPaged(parameters);
            var actors = _mapper.Map<List<ActorDto>>(pagedResult.items);
            var paginationData = _mapper.Map<PaginationDataDto>(pagedResult);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationData));
            return Ok(actors);
        }

        [HttpGet("{actorId}")]
        [ProducesResponseType(200, Type = typeof(ActorDto))]
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
        [ProducesResponseType(200, Type = typeof(ActorDto))]
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<MovieDto>))]
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<MovieActorDto>))]
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
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        public IActionResult CreateActor([FromBody] ActorDto actorCreate)
        {
            if (actorCreate == null)
                return BadRequest();
            if (_actorRepository.ActorExists(actorCreate.Name))
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

            return Ok(actorMap.ActorId);

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
