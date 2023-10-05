using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieAppAPI.Dtos;
using MovieAppAPI.Helper;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;
using MovieAppAPI.Repositories;
using MovieAppAPI.UtilityService;
using System.Security.Cryptography;
using System.Web;

namespace MovieAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IAzureStorageService _azureStorageService;
        public UserController(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher, IWebHostEnvironment hostEnvironment, IConfiguration configuration, IEmailService emailService, IAzureStorageService azureStorageService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _azureStorageService = azureStorageService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        [ProducesResponseType(400)]
        [Authorize]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());
            return Ok(users);
        }

        [HttpGet("username/{username}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetUser(string username)
        {
            var user = _mapper.Map<UserDto>(_userRepository.GetUser(username));
            if (user is null)
            {
                return NotFound("User not found.");
            }
            if (!String.IsNullOrEmpty(user.ImageName))
                user.ImageSource = _azureStorageService.GetFileUrl(user.ImageName);
            return Ok(user);
        }


        [HttpGet("id/{userId}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetUser(int userId)
        {
            var user = _mapper.Map<UserDto>(_userRepository.GetUser(userId));
            if(user is null)
            {
                return NotFound("User not found.");
            }
            if(!String.IsNullOrEmpty(user.ImageName))
               user.ImageSource = _azureStorageService.GetFileUrl(user.ImageName);
            return Ok(user);
        }

        [HttpPost("authenticate")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult AuthenticateUser([FromBody] UserLoginDto userVerify)
        {
            if (userVerify == null)
                return BadRequest();
            var user = _userRepository.GetUserByEmail(userVerify.Email);
            if (user is null || !_passwordHasher.VerifyPassword(userVerify.Password, user.Password))
                return NotFound("Password or email is incorrect.");

            user.Token = _userRepository.CreateJwt(user);

            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPost("register")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult RegisterUser([FromBody] UserSignupDto userCreate)
        {
            if (userCreate == null)
                return BadRequest();

            if (_userRepository.UserExists(userCreate.Email))
            {
                ModelState.AddModelError("email", "Email  already exists.");
                return BadRequest(ModelState);
            }
            if (_userRepository.UsernameExists(userCreate.Username))
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
            var userMap = _mapper.Map<User>(userCreate);
            userMap.Password = _passwordHasher.HashPassword(userMap.Password);
            userMap.Role = "user";

            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public IActionResult UpdateUser(int userId, [FromForm] UserUpdateDto updatedUser)
        {
            if (updatedUser == null)
                return BadRequest();
            if (!_userRepository.UserExists(userId))
                return NotFound();
            var user = _userRepository.GetUser(userId);
            if (!_passwordHasher.VerifyPassword(updatedUser.ConfirmPassword, user.Password))
            {
                ModelState.AddModelError("password", "Password is incorrect.");
                return BadRequest(ModelState);
            }
            if (_userRepository.UsernameExistsUpdate(userId, updatedUser.Username))
            {
                ModelState.AddModelError("username", "Username not available.");
                return BadRequest(ModelState);
            }
            user.Username = updatedUser.Username;
            if (updatedUser.ImageFile != null)
            {
                //if the user adds a new image, delete their previous profile image from the system
                if (!String.IsNullOrEmpty(user.ImageName))
                    _azureStorageService.DeleteImage(user.ImageName);
                user.ImageName = _azureStorageService.UploadImage(updatedUser.ImageFile);
            }
            //if the user wishes only to delete their current image 
            else if (updatedUser.DeleteCurrentImage == true && !String.IsNullOrEmpty(user.ImageName))
            {
                _azureStorageService.DeleteImage(user.ImageName);
                user.ImageName = "";
            }
            if (!_userRepository.UpdateUser(user))
                return BadRequest("Something went wrong while updating the user.");
            user.Token = _userRepository.CreateJwt(user);
            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPost("send-reset-email")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult SendResetEmail([FromQuery]string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            if(user is null)
            {
                return NotFound("Email not found");
            }
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
            var emailModel = new Email(email, "Reset Password", EmailBody.EmailStringBody(email, emailToken, _configuration["EmailSettings:Url"]));
            if(!_userRepository.UpdateUser(user))
            {
                return BadRequest("Something went wrong while sending reset email.");
            }
            _emailService.SendEmail(emailModel);
            return NoContent();
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (resetPasswordDto is null)
            {
                return BadRequest();
            }
            var user = _userRepository.GetUserByEmail(resetPasswordDto.Email);
            if (user is null)
            {
                return NotFound("Email not found.");
            }
            if(user.ResetPasswordToken != resetPasswordDto.EmailToken) 
            {
                return BadRequest("Invalid reset password link.");
            }
            if (user.ResetPasswordExpiry <= DateTime.Now)
            {
                return BadRequest("Reset password link expired.");
            }
            string passwordMessages = _userRepository.CheckPasswordStrength(resetPasswordDto.NewPassword);
            if (!string.IsNullOrEmpty(passwordMessages))
            {
                return BadRequest(passwordMessages);
            }
            user.Password = _passwordHasher.HashPassword(resetPasswordDto.NewPassword);
            if (!_userRepository.UpdateUser(user))
            {
                return BadRequest("Something went wrong while updating password.");
            }
            return NoContent();
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize]
        public IActionResult DeleteUser(int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound();
            var userToDelete = _userRepository.GetUser(userId);
            if (!String.IsNullOrEmpty(userToDelete.ImageName))
            {
                _azureStorageService.DeleteImage(userToDelete.ImageName);
            }
            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting user.");
            }
            return NoContent();

        }

        //used these to upload user images in the file system before using azure blob storage
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
