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
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(users);
        }

        [HttpPost("authenticate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AuthenticateUser([FromBody] UserDto userVerify)
        {
            if (userVerify == null)
                return BadRequest();

            var user = _userRepository.GetUsers()
                        .Where(u => u.Username == userVerify.Username && u.Password == userVerify.Password)
                        .FirstOrDefault();
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] UserDto userCreate)
        {
            if (userCreate == null)
                return BadRequest();

            var user = _userRepository.GetUsers()
                        .Where(u => u.Username == userCreate.Username)
                        .FirstOrDefault();
            if (user != null)
            {
                ModelState.AddModelError("", "Username already exists.");
                return StatusCode(500, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var userMap = _mapper.Map<User>(userCreate);
            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully registered");
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteUser(int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound();
            var userToDelete = _userRepository.GetUser(userId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting user.");

            }
            return NoContent();

        }

    }
}
