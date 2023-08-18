using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieAppAPI.Controllers;
using MovieAppAPI.Dtos;
using MovieAppAPI.Helper;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;
using MovieAppAPI.UtilityService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;

        public UserControllerTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _mapper = A.Fake<IMapper>();
            _hostEnvironment = A.Fake<IWebHostEnvironment>();
            _passwordHasher = A.Fake<IPasswordHasher>();
            _emailService = A.Fake<IEmailService>();
        }

        [Fact]
        public void UserController_RegisterUser_ReturnsOk()
        {
            //Arrange
            var userCreate = A.Fake<UserDto>();
            var user = A.Fake<User>();
            A.CallTo(() => _userRepository.UserExists(userCreate.Email)).Returns(false);
            A.CallTo(() => _userRepository.UsernameExists(userCreate.Username)).Returns(false);
            A.CallTo(() => _userRepository.CheckPasswordStrength(userCreate.Password)).Returns("");
            A.CallTo(() => _mapper.Map<User>(userCreate)).Returns(user);
            //A.CallTo(() => _userRepository.HashUserPassword(user));
            A.CallTo(() => _passwordHasher.HashPassword(user.Password)).Returns("hashed_password");
            A.CallTo(() => _userRepository.CreateUser(user)).Returns(true);
            var controller = new UserController(_userRepository, _mapper, _passwordHasher, _hostEnvironment, _emailService);

            //Act
            var result = controller.RegisterUser(userCreate);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkResult));
        }

        [Fact]
        public void UserController_RegisterUser_ReturnsBadRequest()
        {
            //Arrange
            var userCreate = A.Fake<UserDto>();
            var user = A.Fake<User>();
            A.CallTo(() => _userRepository.UserExists(userCreate.Email)).Returns(false);
            A.CallTo(() => _userRepository.UsernameExists(userCreate.Username)).Returns(true);
            var controller = new UserController(_userRepository, _mapper, _passwordHasher, _hostEnvironment, _emailService);

            //Act
            var result = controller.RegisterUser(userCreate);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public void UserController_RegisterUser_ReturnsBadRequestPassword()
        {
            //Arrange
            var userCreate = A.Fake<UserDto>();
            var user = A.Fake<User>();
            A.CallTo(() => _userRepository.UserExists(userCreate.Email)).Returns(false);
            A.CallTo(() => _userRepository.UsernameExists(userCreate.Username)).Returns(false);
            A.CallTo(() => _userRepository.CheckPasswordStrength(userCreate.Password)).Returns("Password not strong enough.");
            var controller = new UserController(_userRepository, _mapper, _passwordHasher, _hostEnvironment, _emailService);

            //Act
            var result = controller.RegisterUser(userCreate);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public void UserController_AuthenticateUser_ReturnsOk()
        {
            //Arrange
            var userVerify = A.Fake<UserDto>();
            var user = A.Fake<User>();
            A.CallTo(() => _userRepository.GetUserByEmail(userVerify.Email)).Returns(user);
            A.CallTo(() => _passwordHasher.VerifyPassword(userVerify.Password, user.Password)).Returns(true);
            A.CallTo(() => _userRepository.CreateJwt(user));
            var controller = new UserController(_userRepository, _mapper, _passwordHasher, _hostEnvironment, _emailService);

            //Act
            var result = controller.AuthenticateUser(userVerify);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void UserController_AuthenticateUser_ReturnsNotFound()
        {
            //Arrange
            var userVerify = A.Fake<UserDto>();
            var user = A.Fake<User>();
            A.CallTo(() => _userRepository.GetUserByEmail(userVerify.Email)).Returns(user);
            A.CallTo(() => _passwordHasher.VerifyPassword(userVerify.Password, user.Password)).Returns(false);
            var controller = new UserController(_userRepository, _mapper, _passwordHasher, _hostEnvironment, _emailService);

            //Act
            var result = controller.AuthenticateUser(userVerify);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NotFoundObjectResult));
        }
    }
}
