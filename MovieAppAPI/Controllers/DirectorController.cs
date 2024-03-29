﻿
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<DirectorDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetDirectors([FromQuery] string? input)
        {
            var directors = _mapper.Map<List<DirectorDto>>(_directorRepository.GetDirectors(input));
            return Ok(directors);
        }

        [HttpGet("paged")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DirectorDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetMovies([FromQuery] QueryStringParameters parameters)
        {
            var pagedResult = _directorRepository.GetDirectorsPaged(parameters);
            var directors = _mapper.Map<List<DirectorDto>>(pagedResult.items);
            var paginationData = _mapper.Map<PaginationDataDto>(pagedResult);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationData));
            return Ok(directors);
        }


        [HttpGet("{directorId}")]
        [ProducesResponseType(200, Type = typeof(DirectorDto))]
        [ProducesResponseType(400)]
        public IActionResult GetDirector(int directorId)
        {
            if (!_directorRepository.DirectorExists(directorId))
            {
                return NotFound();
            }
            var director = _mapper.Map<DirectorDto>(_directorRepository.GetDirector(directorId));
            return Ok(director);

        }
        [HttpGet("director-name/{directorName}")]
        [ProducesResponseType(200, Type = typeof(DirectorDto))]
        [ProducesResponseType(400)]
        public IActionResult GetDirector(string directorName)
        {
            if (!_directorRepository.DirectorExists(directorName))
            {
                return NotFound();
            }
            var director = _mapper.Map<DirectorDto>(_directorRepository.GetDirector(directorName));
            return Ok(director);

        }

        [HttpGet("movie/{directorId}")]
        [ProducesResponseType(200, Type = typeof(MovieDto))]
        [ProducesResponseType(400)]
        public IActionResult GetDirectorMovie(int directorId)
        {
            var movies = _mapper.Map<List<MovieDto>>(_directorRepository.GetMoviesFromADirector(directorId));
            return Ok(movies);
        }
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        [Authorize(Roles = "admin")]
        public IActionResult CreateDirector([FromBody] DirectorDto directorCreate)
        {
            if (directorCreate == null)
                return BadRequest();
            if (_directorRepository.DirectorExists(directorCreate.Name))
            {
                return BadRequest("Director already exists.");
            }
            var directorMap = _mapper.Map<Director>(directorCreate);
            if (!_directorRepository.CreateDirector(directorMap))
            {
                return BadRequest("Something went wrong while saving.");
            }

            return Ok(directorMap.DirectorId);

        }
        [HttpPut("{directorId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateDirector(int directorId, [FromBody] DirectorDto updatedDirector)
        {
            if (updatedDirector == null)
                return BadRequest(ModelState);
            if (directorId != updatedDirector.DirectorId)
                return BadRequest(ModelState);
            if (!_directorRepository.DirectorExists(directorId))
                return NotFound();

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
        [Authorize(Roles = "admin")]
        public IActionResult DeleteDirector(int directorId)
        {
            if (!_directorRepository.DirectorExists(directorId))
                return NotFound();
           
            var directorToDelete = _directorRepository.GetDirector(directorId);
            if (!_directorRepository.DeleteDirector(directorToDelete))
            {
                return BadRequest("Something went wrong while deleting director.");
            }
            return NoContent();
        }
    }
}
