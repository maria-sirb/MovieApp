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
    public class ActorControllerTests
    {
        private readonly IActorRepository _actorRepository;
        private readonly IMapper _mapper;
        public ActorControllerTests()
        {
            _actorRepository = A.Fake<IActorRepository>();
            _mapper = A.Fake<IMapper>(); 
        }

        [Fact]
        public void ActorController_getActors_ReturnOk()
        {
            //Arrange
            var input = "";
            var actors = A.Fake<ICollection<Actor>>();
            var actorsList = A.Fake<List<ActorDto>>();
            A.CallTo(() => _actorRepository.GetActors(input)).Returns(actors);
            A.CallTo(() => _mapper.Map<List<ActorDto>>(actors)).Returns(actorsList);
            var controller = new ActorController(_actorRepository, _mapper);

            //Act
            var result = controller.GetActors(input);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void ActorController_createActor_ReturnOk()
        {
            //Arrange
            var actorCreate = A.Fake<ActorDto>();
            var actor = A.Fake<Actor>();
            A.CallTo(() => _actorRepository.ActorExists(actorCreate.Name)).Returns(false);
            A.CallTo(() => _mapper.Map<Actor>(actorCreate)).Returns(actor);
            A.CallTo(() => _actorRepository.CreateActor(actor)).Returns(true);
            var controller = new ActorController(_actorRepository, _mapper);

            //Act
            var result = controller.CreateActor(actorCreate);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void ActorController_createActor_ReturnBadRequest()
        {
            //Arrange
            var actorCreate = A.Fake<ActorDto>();
            var actor = A.Fake<Actor>();
            A.CallTo(() => _actorRepository.ActorExists(actorCreate.Name)).Returns(true);
            A.CallTo(() => _mapper.Map<Actor>(actorCreate)).Returns(actor);
            A.CallTo(() => _actorRepository.CreateActor(actor)).Returns(true);
            var controller = new ActorController(_actorRepository, _mapper);

            //Act
            var result = controller.CreateActor(actorCreate);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public void ActorController_updateActor_ReturnOk()
        {
            var actorUpdate = A.Fake<ActorDto>();
            var actor = A.Fake<Actor>();
            var actorId = actorUpdate.ActorId;
            A.CallTo(() => _actorRepository.ActorExists(actorUpdate.ActorId)).Returns(true);
            A.CallTo(() => _mapper.Map<Actor>(actorUpdate)).Returns(actor);
            A.CallTo(() => _actorRepository.UpdateActor(actor)).Returns(true);
            var controller = new ActorController(_actorRepository, _mapper);

            //Act
            var result = controller.UpdateActor(actorId, actorUpdate);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NoContentResult));
        }

        [Fact]
        public void ActorController_updateActor_ReturnBadRequest()
        {
            var actorUpdate = A.Fake<ActorDto>();
            var actor = A.Fake<Actor>();
            var actorId = actorUpdate.ActorId + 1;
            A.CallTo(() => _actorRepository.ActorExists(actorUpdate.ActorId)).Returns(true);
            A.CallTo(() => _mapper.Map<Actor>(actorUpdate)).Returns(actor);
            A.CallTo(() => _actorRepository.UpdateActor(actor)).Returns(true);
            var controller = new ActorController(_actorRepository, _mapper);

            //Act
            var result = controller.UpdateActor(actorId, actorUpdate);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }
    }
}
