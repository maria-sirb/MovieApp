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
        private readonly IWebHostEnvironment _hostEnvironment;
        public UserController(IUserRepository userRepository, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
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
            if(!String.IsNullOrEmpty(user.ImageName))
                user.ImageSource = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/images/{user.ImageName}";
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
                return BadRequest(ModelState);
            }

            var user = _userRepository.GetUsers()
                        .Where(u => u.Username == userCreate.Username)
                        .FirstOrDefault();
            if (user != null)
            {
                 ModelState.AddModelError("username", "Username not available.");
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

        [HttpPut("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult UpdateUser(int userId, [FromForm]UserUpdateDto updatedUser)
        {
            if (updatedUser == null)
                return BadRequest();
            if (userId != updatedUser.UserId)
                return BadRequest();
            if (!_userRepository.UserExists(userId))
                return NotFound();
            var userMap = _mapper.Map<User>(updatedUser);
            if(_userRepository.UsernameExistsUpdate(userId, userMap.Username))
            {
                ModelState.AddModelError("username", "Username not available.");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest();
            if (updatedUser.ImageFile != null)
            {
                //if the user adds a new image, delete their previous profile image from the system
                if (!String.IsNullOrEmpty(updatedUser.ImageName))
                    DeleteImage(updatedUser.ImageName);
                userMap.ImageName = SaveImage(updatedUser.ImageFile);
            }
            //if the user wishes only to delete their current image 
            else if (updatedUser.ImageFile == null && String.IsNullOrEmpty(updatedUser.ImageSource) && !String.IsNullOrEmpty(updatedUser.ImageName))
            {
                DeleteImage(updatedUser.ImageName);
                userMap.ImageName = "";
            }
            if (!_userRepository.UpdateUser(userMap))
                return BadRequest("Something went wrong while updating the user.");
            userMap.Token = _userRepository.CreateJwt(userMap);
            return Ok(userMap);
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
        [NonAction]
        public string SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-')
                                + DateTime.Now.ToString("yymmddssfff") + Path.GetExtension(imageFile.FileName);
            string imagePath = Path.Combine(_hostEnvironment.ContentRootPath,"Images", imageName);
            using(FileStream fileStream  = new FileStream(imagePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }
            return imageName;

        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            string imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

        }
    }
}
