using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MovieAppAPI.Controllers;
using MovieAppAPI.Dtos;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Tests.Controllers
{
    public class MovieControllerTests
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IDirectorRepository _directorRepository;
        private readonly IMapper _mapper;
        public MovieControllerTests()
        {
            _movieRepository = A.Fake<IMovieRepository>();
            _directorRepository = A.Fake<IDirectorRepository>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public void MovieController_GetMovies_ReturnOk()
        {
            //Arrange
            var input = "";
            var movies = A.Fake<ICollection<Movie>>();
            var movieList = A.Fake<List<MovieDto>>();
            A.CallTo(() => _movieRepository.GetMovies(input)).Returns(movies);
            A.CallTo(() => _mapper.Map<List<MovieDto>>(movies)).Returns(movieList);
            var controller = new MovieController(_movieRepository, _directorRepository, _mapper);

            //Act
            var result = controller.GetMovies(input);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void MovieController_GetMovie_ReturnOk()
        {
            //Arrange
            var movieId = 1;
            var movie = A.Fake<Movie>();
            A.CallTo(() => _movieRepository.MovieExists(movieId)).Returns(true);
            A.CallTo(() => _movieRepository.GetMovie(movieId)).Returns(movie);
            var controller = new MovieController(_movieRepository, _directorRepository, _mapper);

            //Assert
            var result = controller.GetMovie(movieId);

            //Act
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void MovieController_GetMovie_ReturnNotFound()
        {
            //Arrange
            var movieId = 1;
            var movie = A.Fake<Movie>();
            A.CallTo(() => _movieRepository.MovieExists(movieId)).Returns(false);
            A.CallTo(() => _movieRepository.GetMovie(movieId)).Returns(movie);
            var controller = new MovieController(_movieRepository, _directorRepository, _mapper);

            //Assert
            var result = controller.GetMovie(movieId);

            //Act
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public void MovieController_CreateMovie_ReturnOk()
        {
            //Arrange
            var directorId = 1;
            var genreIds = new List<int>{ 1, 2, 3};
            var movieCreate = A.Fake<MovieDto>();
            var movieMap = A.Fake<Movie>();
            var director = A.Fake<Director>();
            A.CallTo(() => _movieRepository.MovieExists(movieCreate.Title)).Returns(false);
            A.CallTo(() => _mapper.Map<Movie>(movieCreate)).Returns(movieMap);
            A.CallTo(() => _directorRepository.GetDirector(directorId)).Returns(director);
            A.CallTo(() => _movieRepository.CreateMovie(genreIds, movieMap)).Returns(true);
            var controller = new MovieController(_movieRepository, _directorRepository, _mapper);

            //Act
            var result = controller.CreateMovie(directorId, genreIds, movieCreate);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkResult));
        }

        [Fact]
        public void MovieController_CreateMovie_ReturnBadRequest()
        {
            //Arrange
            var directorId = 1;
            var genreIds = new List<int> { 1, 2, 3 };
            var movieCreate = A.Fake<MovieDto>();
            var movieMap = A.Fake<Movie>();
            var director = A.Fake<Director>();
            A.CallTo(() => _movieRepository.MovieExists(movieCreate.Title)).Returns(true);
            A.CallTo(() => _mapper.Map<Movie>(movieCreate)).Returns(movieMap);
            A.CallTo(() => _directorRepository.GetDirector(directorId)).Returns(director);
            A.CallTo(() => _movieRepository.CreateMovie(genreIds, movieMap)).Returns(true);
            var controller = new MovieController(_movieRepository, _directorRepository, _mapper);

            //Act
            var result = controller.CreateMovie(directorId, genreIds, movieCreate);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public void MovieController_UpdateMovie_ReturnOk()
        {
            //Arrange
            var directorId = 1;
            var genreIds = new List<int> { 1, 2, 3 };
            var movieUpdate = A.Fake<MovieDto>();
            var movieMap = A.Fake<Movie>();
            var movieId = movieMap.MovieId;
            var director = A.Fake<Director>();
            A.CallTo(() => _movieRepository.MovieExists(movieId)).Returns(true);
            A.CallTo(() => _mapper.Map<Movie>(movieUpdate)).Returns(movieMap);
            A.CallTo(() => _directorRepository.GetDirector(directorId)).Returns(director);
            A.CallTo(() => _movieRepository.UpdateMovie(genreIds, movieMap)).Returns(true);
            var controller = new MovieController(_movieRepository, _directorRepository, _mapper);

            //Act
            var result = controller.UpdateMovie(movieId, directorId, genreIds, movieUpdate);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NoContentResult));
        }

        [Fact]
        public void MovieController_UpdateMovie_ReturnBadRequest()
        {
            //Arrange
            var directorId = 1;
            var genreIds = new List<int> { 1, 2, 3 };
            var movieUpdate = A.Fake<MovieDto>();
            var movieMap = A.Fake<Movie>();
            var movieId = movieMap.MovieId + 1;
            var director = A.Fake<Director>();
            A.CallTo(() => _movieRepository.MovieExists(movieId)).Returns(true);
            A.CallTo(() => _mapper.Map<Movie>(movieUpdate)).Returns(movieMap);
            A.CallTo(() => _directorRepository.GetDirector(directorId)).Returns(director);
            A.CallTo(() => _movieRepository.UpdateMovie(genreIds, movieMap)).Returns(true);
            var controller = new MovieController(_movieRepository, _directorRepository, _mapper);

            //Act
            var result = controller.UpdateMovie(movieId, directorId, genreIds, movieUpdate);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }
    }
}
