using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieAppAPI.Dtos;
using MovieAppAPI.Helper;
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        [ProducesResponseType(400)]
        [Authorize]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(users);
        }

        [HttpGet("username/{username}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(string username)
        {
            var user = _mapper.Map<UserDto>(_userRepository.GetUser(username));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(user);
        }


        [HttpGet("id/{userId}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(int userId)
        {
            var user = _mapper.Map<UserDto>(_userRepository.GetUser(userId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        [HttpPost("authenticate")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult AuthenticateUser([FromBody] UserDto userVerify)
        {
            if (userVerify == null)
                return BadRequest();

            var user = _userRepository.GetUsers()
                        .Where(u => u.Email == userVerify.Email && PasswordHasher.VerifyPassword(userVerify.Password, u.Password))
                        .FirstOrDefault();
            if (user == null)
                return NotFound("Password or email is incorrect.");

            user.Token = _userRepository.CreateJwt(user);

            return Ok(user);
        }

        [HttpPost("register")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult RegisterUser([FromBody] UserDto userCreate)
        {
            if (userCreate == null)
                return BadRequest();

            if (_userRepository.UserExists(userCreate.Email))
            {
                ModelState.AddModelError("email", "Email  already exists.");
                // return StatusCode(500, ModelState);
                // return BadRequest("Email already exists.");
                return BadRequest(ModelState);
            }

            var user = _userRepository.GetUsers()
                        .Where(u => u.Username == userCreate.Username)
                        .FirstOrDefault();
            if (user != null)
            {
                 ModelState.AddModelError("username", "Username not available.");
                // return StatusCode(500, ModelState);
                return BadRequest(ModelState);
            }

            string passwordMessages = _userRepository.CheckPasswordStrength(userCreate.Password);
            if (!string.IsNullOrEmpty(passwordMessages))
            {
                ModelState.AddModelError("password", passwordMessages);
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var userMap = _mapper.Map<User>(userCreate);
            userMap.Password = PasswordHasher.HashPassword(userMap.Password);
            userMap.Token = "";
            if(string.IsNullOrEmpty(userCreate.Role))
                userMap.Role = "user";
            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }
            return Ok();
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
